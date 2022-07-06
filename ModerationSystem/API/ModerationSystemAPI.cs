using System.Collections.Generic;
using System.Globalization;
using Exiled.Permissions.Extensions;
using InventorySystem.Items.Usables.Scp244;
using Respawning;

namespace ModerationSystem.API
{
    using System;
    using System.Linq;
    using System.Text;
    using Exiled.API.Features;
    using JetBrains.Annotations;
    using ModerationSystem.Collections;
    using ModerationSystem.Enums;
    using ModerationSystem.GlobalDatabase;
    using Newtonsoft.Json;
    using NorthwoodLib.Pools;
    using static Database.Database;
    using Player = Exiled.API.Features.Player;
    using MEC;
    using ModerationSystem.Discord;

    public static class ModerationSystemAPI
    {
        /// <summary>
        /// Apply a punish to the specified <see cref="Player"/>.
        /// </summary>
        /// <param name="target">The <see cref="Exiled.API.Features.Player"/> player.</param>
        /// <param name="issuer">The <see cref="Collections.Player"/> staffer.</param>
        /// <param name="dPlayer">The <see cref="Collections.Player"/> player.</param>
        /// <param name="punishType">The <see cref="Enums.PunishType"/> punish.</param>
        /// <param name="reason">The reason of the punish.</param>
        /// <param name="duration">The <see cref="DateTime"/> duration.</param>
        public static void ApplyPunish(Player target, Collections.Player issuer, Collections.Player dPlayer, PunishType punishType, string reason, string duration)
        {
            switch (punishType)
            {
                case PunishType.Warn:
                    Warn warn = new Warn(dPlayer, issuer, reason, DateTime.Now, WarnCollection.Find(w => w.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    warn.Save();
                    target?.Broadcast(Plugin.Singleton.Config.Translation.WarnTranslation.PlayerWarnedMessage.Duration,
                        Plugin.Singleton.Config.Translation.WarnTranslation.PlayerWarnedMessage.Content.Replace(
                            "{reason}", reason), global::Broadcast.BroadcastFlags.Normal, true);
                    
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(warn), dPlayer, ActionType.Add);
                    break;

                case PunishType.Mute:
                    Mute mute = new Mute(dPlayer, issuer, reason, GetDate(duration), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration).TotalSeconds), MuteCollection.Find(m => m.Target == dPlayer).Count(), Server.Port, false);
                    mute.Save();

                    MuteHandler.IssuePersistentMute(dPlayer.Id + dPlayer.Authentication);
                    target?.Broadcast(Plugin.Singleton.Config.Translation.MuteTranslation.PlayerMuteMessage.Duration,
                        Plugin.Singleton.Config.Translation.WarnTranslation.PlayerWarnedMessage.Content
                            .Replace("{duration}", GetDate(duration)).Replace("{reason}", reason));

                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(mute), dPlayer, ActionType.Add);
                    break;

                case PunishType.Kick:
                    Kick kick = new Kick(dPlayer, issuer, reason, DateTime.Now, KickCollection.Find(x => x.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    kick.Save();

                    target?.Kick(Plugin.Singleton.Config.Translation.KickTranslation.PlayerKickedMessage.Replace("{reason}", reason));

                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(kick), dPlayer, ActionType.Add);
                    break;

                case PunishType.Ban:
                    Ban ban = new Ban(dPlayer, issuer, reason, GetDate(duration, true), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration, true).TotalSeconds), BanCollection.Find(x => x.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    ban.Save();
                    target?.Disconnect(Plugin.Singleton.Config.Translation.BanTranslation.PlayerBanMessage.Replace("{reason}", reason));
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(ban), dPlayer, ActionType.Add);
                    break;

                case PunishType.SoftWarn:
                    SoftWarn softWarn = new SoftWarn(dPlayer, issuer, reason, DateTime.Now, SoftWarnCollection.Find(sw => sw.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    softWarn.Save();

                    target?.Broadcast(
                        Plugin.Singleton.Config.Translation.SoftWarnTranslation.PlayerSoftWarnedMessage.Duration,
                        Plugin.Singleton.Config.Translation.SoftWarnTranslation.PlayerSoftWarnedMessage.Content.Replace(
                            "{reason}", reason));

                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(softWarn), dPlayer,
                        ActionType.Add);
                    break;

                case PunishType.SoftBan:
                    SoftBan softBan = new SoftBan(dPlayer, issuer, reason, GetDate(duration), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration).TotalSeconds), SoftBanCollection.Find(sb => sb.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    softBan.Save();

                    target?.Broadcast(
                        Plugin.Singleton.Config.Translation.SoftBanTranslation.PlayerSoftBanMessage.Duration,
                        Plugin.Singleton.Config.Translation.StaffTranslation.StaffSoftBanMessage.Content.Replace(
                            "{duration}",
                            GetDate(duration).Replace("{reason}", reason)));

                    if (target != null && target.Role != RoleType.Spectator) target.Role.Type = RoleType.Spectator;

                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(softBan), dPlayer, ActionType.Add);
                    break;

                case PunishType.WatchList:
                    WatchList watchList = new WatchList(dPlayer, issuer, reason, DateTime.Now, WatchListCollection.Find(wl => wl.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    watchList.Save();

                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(watchList), dPlayer, ActionType.Add);
                    break;
            }
            
            if (punishType is PunishType.WatchList)
                Timing.RunCoroutine(DiscordHandler.SendMessage(Plugin.Singleton.Config.Translation.DiscordTranslation.MessageContentWatchlist.Replace("{target}", $"{dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication})").Replace("{reason}", reason).Replace("{issuer}", $"{issuer.Name} ({issuer.Id}@{issuer.Authentication})"), Plugin.Singleton.Config.Translation.DiscordTranslation.WebhookUrlWatchlist));

            Timing.RunCoroutine(DiscordHandler.SendMessage(Plugin.Singleton.Config.Translation.DiscordTranslation.MessageContent.Replace("{target}", $"{dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication})").Replace("{reason}", reason).Replace("{action}", punishType.ToString()).Replace("{issuer}", $"{issuer.Name} ({issuer.Id}@{issuer.Authentication})").Replace("{duration}", GetDate(duration)), Plugin.Singleton.Config.Translation.DiscordTranslation.WebhookUrl));

        }

        /// <summary>
        /// Clear all punishment from player <see cref="Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="Collections.Player"/> player.</param>
        public static void Clear(this Collections.Player player)
        {
            BanCollection.DeleteMany(p => p.Target == player);
            MuteCollection.DeleteMany(p => p.Target == player);
            KickCollection.DeleteMany(p => p.Target == player);
            WarnCollection.DeleteMany(p => p.Target == player);
            SoftBanCollection.DeleteMany(p => p.Target == player);
            SoftWarnCollection.DeleteMany(p => p.Target == player);
            WatchListCollection.DeleteMany(p => p.Target == player);
            JsonManager.PunishToCache(PunishType.All, JsonConvert.SerializeObject(player), player, ActionType.Remove);
        }

        /// <summary>
        /// Clear all punishment from <see cref="Player"/>.
        /// </summary>
        /// <param name="punish"> the punish name.</param>
        /// <returns> the punish using the name.</returns>
        public static PunishType? GetPunishType(this string punish)
        {
            switch (punish)
            {
                case "ban":
                    return PunishType.Ban;
                case "kick":
                    return PunishType.Kick;
                case "mute":
                    return PunishType.Mute;
                case "warn":
                    return PunishType.Warn;
                case "softwarn":
                    return PunishType.SoftWarn;
                case "softban":
                    return PunishType.SoftBan;
                default: return null;
            }
        }

        /// <summary>
        /// Clear a punish to the specified <see cref="Player"/>.
        /// </summary>
        /// <param name="player">The <see cref="Collections.Player"/> player.</param>
        /// <param name="type"> the <see cref="Enums.PunishType"/>.</param>
        /// <param name="id"> the punish id.</param>
        /// <param name="server"> the server port.</param>
        public static void ClearPunishment(Collections.Player player, PunishType? type, int id, int server)
        {
            switch (type)
            {
                case PunishType.Ban:
                    Ban ban = BanCollection.FindOne(b => b.Target == player && b.BanId == id && b.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(ban), player, ActionType.Remove);

                    if (player.IsBanned())
                        BanHandler.RemoveBan($"{player.Id}@{player.Authentication}", BanHandler.BanType.IP);
    
                    BanCollection.Delete(ban.Id);
                    break;

                case PunishType.Kick:
                    Kick kick = KickCollection.FindOne(k => k.Target == player && k.KickId == id && k.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(kick), player, ActionType.Remove);

                    KickCollection.Delete(kick.Id);
                    break;

                case PunishType.Mute:
                    Mute mute = MuteCollection.FindOne(m => m.Target == player && m.MuteId == id && m.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(mute), player, ActionType.Remove);

                    if (player.IsMuted())
                        MuteHandler.RevokePersistentMute($"{player.Id}@{player.Authentication}");

                    MuteCollection.Delete(mute.Id);
                    break;

                case PunishType.Warn:
                    Warn warn = WarnCollection.FindOne(w => w.Target == player && w.WarnId == id && w.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(warn), player, ActionType.Remove);

                    WarnCollection.Delete(warn.Id);
                    break;

                case PunishType.SoftWarn:
                    SoftWarn softWarn = SoftWarnCollection.FindOne(sw => sw.Target == player && sw.SoftWarnId == id && sw.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(softWarn), player, ActionType.Remove);

                    SoftWarnCollection.Delete(softWarn.Id);
                    break;

                case PunishType.SoftBan:
                    SoftBan softBan = SoftBanCollection.FindOne(sb => sb.Target == player && sb.SoftBanId == id && sb.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(softBan), player, ActionType.Remove);

                    SoftBanCollection.Delete(softBan.Id);
                    break;

                case PunishType.WatchList:
                    WatchList watchList = WatchListCollection.FindOne(wl => wl.Target == player && wl.WatchListId == id && wl.Server == server);

                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(watchList), player, ActionType.Remove);

                    WatchListCollection.Delete(watchList.Id);
                    break;
            }
        }

        /// <summary>
        /// Gets the specified <see cref="Collections.Player"/>.
        /// </summary>
        /// <param name="player">The nickname or steamid64</param>
        public static Collections.Player GetPlayer(this string player) => Player.Get(player)?.GetPlayer() ?? PlayerCollection.Query().Where(qPlayer => qPlayer.Id == player.GetRawUserId() || qPlayer.Name == player).FirstOrDefault();

        /// <summary>
        /// Gets the specified <see cref="Collections.Player"/>.
        /// </summary>
        /// <param name="player">The specified <see cref="Exiled.API.Features.Player"/>.</param>
        public static Collections.Player GetPlayer(this Player player)
        {
            if (player == null || string.IsNullOrEmpty(player.UserId) && !player.IsHost) return null;

            if (player.IsHost) return ServerPlayer;

            if (PlayerData.TryGetValue(player, out Collections.Player dPlayer)) return dPlayer;

            return PlayerCollection.FindById(player.RawUserId);
        }

        /// <summary>
        /// Gets the specified <see cref="Collections.Player"/> staffer.
        /// </summary>
        /// <param name="sender">The specified CommandSender.</param>
        public static Collections.Player GetStaffer(this CommandSender sender)
        {
            return new Collections.Player
            (
                sender?.SenderId?.GetRawUserId() ?? "Server",
                sender?.SenderId?.GetAuthentication() ?? "Server",
                sender?.Nickname ?? "Server"
            );
        }

        /// <summary>
        /// Check if exists a specified punish.
        /// </summary>
        /// <param name="player">The <see cref="Collections.Player"/> player.</param>
        /// <param name="type"> the <see cref="Enums.PunishType"/>.</param>
        /// <param name="id"> the punish id.</param>
        /// <param name="server"> the server port.</param>
        /// <returns> true if the punish exists, false if not.</returns>
        public static bool CheckId(Collections.Player player, PunishType? type, int id, int server)
        {
            switch (type)
            {
                case PunishType.Ban:
                    return BanCollection.Exists(ban => ban.Target == player && ban.BanId == id && ban.Server == server);

                case PunishType.Kick:
                    return KickCollection.Exists(kick => kick.Target == player && kick.KickId == id && kick.Server == server);

                case PunishType.Mute:
                    return MuteCollection.Exists(mute => mute.Target == player && mute.MuteId == id && mute.Server == server);

                case PunishType.Warn:
                    return WarnCollection.Exists(warn => warn.Target == player && warn.WarnId == id && warn.Server == server);

                case PunishType.SoftWarn:
                    return SoftWarnCollection.Exists(sw => sw.Target == player && sw.SoftWarnId == id && sw.Server == server);

                case PunishType.SoftBan:
                    return SoftBanCollection.Exists(sb => sb.Target == player && sb.SoftBanId == id && sb.Server == server);

                case PunishType.WatchList:
                    return WatchListCollection.Exists(wl => wl.Target == player && wl.WatchListId == id && wl.Server == server);

                default: return false;
                
            }
        }

        /// <summary>
        /// Gets a value indicating wherever or not the specified <see cref="Collections.Player"/> is muted.
        /// </summary>
        /// <param name="dPlayer"> the specified <see cref="Collections.Player"/>.</param>
        /// <returns> true if is muted, false if not.</returns>
        public static bool IsMuted(this Collections.Player dPlayer) => MuteCollection.Exists(mute => mute.Target.Id == dPlayer.Id && mute.Expire > DateTime.Now);

        /// <summary>
        /// Gets a value indicating wherever or not the specified <see cref="Collections.Player"/> is banned.
        /// </summary>
        /// <param name="dPlayer"> the specified <see cref="Collections.Player"/>.</param>
        /// <returns> true if is banned, false if not.</returns>
        public static bool IsBanned(this Collections.Player dPlayer) => BanCollection.Exists(ban => ban.Target.Id == dPlayer.Id && ban.Expire > DateTime.Now);

        /// <summary>
        /// Gets a value indicating wherever or not the specified <see cref="Collections.Player"/> is soft-banned.
        /// </summary>
        /// <param name="dPlayer"> the specified <see cref="Collections.Player"/>.</param>
        /// <returns> true if is soft-banned, false if not.</returns>
        public static bool IsSoftBanned(this Collections.Player dPlayer) => SoftBanCollection.Exists(sb => sb.Target.Id == dPlayer.Id && sb.Expire > DateTime.Now);

        /// <summary>
        /// Get all the watchlist or <see cref="Collections.Player"/> watchlist.
        /// </summary>
        /// <param name="player">The <see cref="Collections.Player"/> player, but can be null.</param>
        /// <returns> the watchlist.</returns>
        public static string GetWatchList([CanBeNull] Collections.Player player)
        {
            StringBuilder text = StringBuilderPool.Shared.Rent().AppendLine();

            if (player == null)
            {
                text.AppendLine($"WatchList ({WatchListCollection.Count()})").AppendLine();
                foreach (WatchList wl in WatchListCollection.FindAll().ToList())
                {
                    text.AppendLine($"Target: {wl.Target.Name} ({wl.Target.Id}@{wl.Target.Authentication})")
                        .AppendLine($"Issuer: {wl.Issuer.Name} ({wl.Issuer.Id}@{wl.Issuer.Authentication})")
                        .AppendLine($"Reason: {wl.Reason}").AppendLine($"ID: {wl.WatchListId}")
                        .AppendLine($"Date: {wl.Date}").AppendLine($"Server sender Port: {wl.Server}").AppendLine();
                }

                return StringBuilderPool.Shared.ToStringReturn(text);
            }

            text.AppendLine($"WatchList ({player.Name} - {player.Id}@{player.Authentication})").AppendLine();

            foreach (WatchList wl in WatchListCollection.Find(wl => wl.Target.Id == player.Id).ToList())
            {
                text.AppendLine($"Target: {wl.Target.Name} ({wl.Target.Id}@{wl.Target.Authentication})")
                    .AppendLine($"Issuer: {wl.Issuer.Name} ({wl.Issuer.Id}@{wl.Issuer.Authentication})")
                    .AppendLine($"Reason: {wl.Reason}").AppendLine($"ID: {wl.WatchListId}")
                    .AppendLine($"Date: {wl.Date}").AppendLine($"Server sender Port: {wl.Server}").AppendLine();
            }

            return StringBuilderPool.Shared.ToStringReturn(text);
        }

        internal static DateTime? ConvertToDateTime(string stringDuration)
        {
            if (stringDuration is null)
                return null;

            int[] durationSplit = Array.ConvertAll(stringDuration.Split(':'), int.Parse);

            if (durationSplit.Length < 4)
                return null;

            return DateTime.Now.AddDays(durationSplit.ElementAt(0)).AddHours(durationSplit.ElementAt(1)).AddMinutes(durationSplit.ElementAt(2)).AddSeconds(durationSplit.ElementAt(3));
        }

        internal static string GetDate(string duration, bool isRa = false)
        {
            string[] durationSplit = duration.Split(':');
            if (isRa)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(Convert.ToInt32(duration));
                return $"{timeSpan.Days}d:{timeSpan.Hours}h:{timeSpan.Minutes}m:{timeSpan.Seconds}s";
            }

            return duration != DateTime.MinValue.ToString(CultureInfo.InvariantCulture) ? $"{durationSplit.ElementAt(0)}d:{durationSplit.ElementAt(1)}h:{durationSplit.ElementAt(2)}m:{durationSplit.ElementAt(3)}s" : "//";
        }

        internal static void SendBroadcast(Broadcast broadcast)
        {
            foreach (Player player in Player.Get(p => p.RemoteAdminAccess))
                player.Broadcast(broadcast, true);
        }

        internal static bool MaxDuration(string duration, Player staffer)
        {
            foreach (KeyValuePair<string, int> permission in Plugin.Singleton.Config.StaffersDurationPermission.Where(dur => dur.Value > GetTotalSeconds(duration).TotalSeconds / 60))
            {
                if (!staffer.CheckPermission(permission.Key)) continue;
                return false;
            }

            return true;
        }

        internal static TimeSpan GetTotalSeconds(string timeString, bool isRa = false)
        {
            if (isRa)
            {
                TimeSpan duration = DateTime.Now.AddSeconds(Convert.ToInt32(timeString)) - DateTime.Now;
                return duration;
            }

            int[] durationSplit = Array.ConvertAll(timeString.Split(':'), int.Parse);

            TimeSpan dur = DateTime.Now.AddDays(durationSplit.ElementAt(0)).AddHours(durationSplit.ElementAt(1)).AddMinutes(durationSplit.ElementAt(2)).AddSeconds(durationSplit.ElementAt(3)) - DateTime.Now;
            return dur;
        }

        private static string GetRawUserId(this string userId)
        {
            int index = userId.LastIndexOf('@');
            return index == -1 ? userId : userId.Substring(0, index);
        }

        private static string GetAuthentication(this string userId) => userId.Substring(userId.LastIndexOf('@') + 1);
    }
}
