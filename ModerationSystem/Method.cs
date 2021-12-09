using System;
using System.Linq;
using Exiled.API.Features;
using JetBrains.Annotations;
using ModerationSystem.Collections;
using ModerationSystem.Enums;
using ModerationSystem.GlobalDatabase;
using Newtonsoft.Json;
using NorthwoodLib.Pools;
using static ModerationSystem.Database;
using Player = Exiled.API.Features.Player;

namespace ModerationSystem
{
    public static class Method
    {
        internal static void ApplyPunish(Player target, Collections.Player issuer, Collections.Player dPlayer,
            PunishType punishType, string reason, DateTime duration)
        {
            switch (punishType)
            {
                case PunishType.Warn:
                    var warn = new Warn(dPlayer, issuer, reason, DateTime.Now,
                        WarnCollection.Find(w => w.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    warn.Save();
                    target?.Broadcast(Plugin.Singleton.Config.Translation.WarnTranslation.PlayerWarnedMessage.Duration,
                        Plugin.Singleton.Config.Translation.WarnTranslation.PlayerWarnedMessage.Content.Replace(
                            "{reason}", reason));
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(warn), dPlayer, ActionType.Add);
                    break;

                case PunishType.Mute:
                    var mute = new Mute(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now,
                        DateTime.Now.AddSeconds(GetTotalSeconds(duration)),
                        MuteCollection.Find(m => m.Target == dPlayer).Count(), Server.Port, false);
                    mute.Save();
                    MuteHandler.IssuePersistentMute(dPlayer.Id + dPlayer.Authentication);
                    target?.Broadcast(Plugin.Singleton.Config.Translation.MuteTranslation.PlayerMuteMessage.Duration,
                        Plugin.Singleton.Config.Translation.WarnTranslation.PlayerWarnedMessage.Content
                            .Replace("{duration}", duration.ToString("HH:mm:ss")).Replace("{reason}", reason));
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(mute), dPlayer, ActionType.Add);
                    break;

                case PunishType.Kick:
                    var kick = new Kick(dPlayer, issuer, reason, DateTime.Now,
                        KickCollection.Find(x => x.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    kick.Save();
                    target?.Kick(
                        Plugin.Singleton.Config.Translation.KickTranslation.PlayerKickedMessage.Replace("{reason}",
                            reason));
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(kick), dPlayer, ActionType.Add);
                    break;

                case PunishType.Ban:
                    var ban = new Ban(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now,
                        DateTime.Now.AddSeconds(GetTotalSeconds(duration)),
                        BanCollection.Find(x => x.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    ban.Save();
                    var details = new BanDetails
                    {
                        Expires = GetTotalSeconds(duration),
                        Id = dPlayer.Id + dPlayer.Authentication,
                        IssuanceTime = DateTime.Now.Ticks,
                        Issuer = issuer.Name,
                        OriginalName = dPlayer.Name,
                        Reason = reason
                    };
                    BanHandler.IssueBan(details, BanHandler.BanType.IP);
                    target?.Disconnect(
                        Plugin.Singleton.Config.Translation.BanTranslation.PlayerBanMessage
                            .Replace("{reason}", reason));
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(ban), dPlayer, ActionType.Add);
                    break;
                case PunishType.SoftWarn:
                    var softWarn = new SoftWarn(dPlayer, issuer, reason, DateTime.Now,
                        SoftWarnCollection.Find(sw => sw.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    softWarn.Save();
                    target?.Broadcast(
                        Plugin.Singleton.Config.Translation.SoftWarnTranslation.PlayerSoftWarnedMessage.Duration,
                        Plugin.Singleton.Config.Translation.StaffTranslation.StaffSoftWarnMessage.Content.Replace(
                            "{reason}", reason));
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(softWarn), dPlayer,
                        ActionType.Add);
                    break;
                case PunishType.SoftBan:
                    var softBan = new SoftBan(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now,
                        DateTime.Now.AddSeconds(GetTotalSeconds(duration)),
                        SoftBanCollection.Find(sb => sb.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    softBan.Save();
                    target?.Broadcast(
                        Plugin.Singleton.Config.Translation.SoftBanTranslation.PlayerSoftBanMessage.Duration,
                        Plugin.Singleton.Config.Translation.StaffTranslation.StaffSoftBanMessage.Content.Replace(
                            "{duration}",
                            duration.ToString("HH:mm:ss").Replace("{reason}", reason)));
                    if (target != null) target.Role = RoleType.Spectator;
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(softBan), dPlayer,
                        ActionType.Add);
                    break;
                case PunishType.WatchList:
                    var watchList = new WatchList(dPlayer, issuer, reason, DateTime.Now,
                        WatchListCollection.Find(wl => wl.Target.Id == dPlayer.Id).Count(), Server.Port, false);
                    watchList.Save();
                    JsonManager.PunishToCache(punishType, JsonConvert.SerializeObject(watchList), dPlayer, ActionType.Add);
                    break;
            }
        }

        internal static void SendBroadcast(Exiled.API.Features.Broadcast broadcast)
        {
            foreach (var player in Player.List.Where(p => p.RemoteAdminAccess)) player.Broadcast(broadcast, true);
        }

        private static int GetTotalSeconds(DateTime time) => (time.Hour * 3600) + (time.Minute * 60) + time.Second;

        internal static void Clear(this Collections.Player player)
        {
            BanCollection.DeleteMany(p => p.Target == player);
            MuteCollection.DeleteMany(p => p.Target == player);
            KickCollection.DeleteMany(p => p.Target == player);
            WarnCollection.DeleteMany(p => p.Target == player);
            SoftBanCollection.DeleteMany(p => p.Target == player);
            SoftWarnCollection.DeleteMany(p => p.Target == player);
            JsonManager.PunishToCache(PunishType.All, JsonConvert.SerializeObject(player), player, ActionType.Remove);
        }

        internal static PunishType? GetPunishType(this string punish)
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

        internal static void ClearPunishment(Collections.Player player, PunishType? type, int id, int server)
        {
            switch (type)
            {
                case PunishType.Ban:
                    var ban = BanCollection.FindOne(b => b.Target == player && b.BanId == id && b.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(ban), player, ActionType.Remove);
                    if (player.IsBanned())
                        BanHandler.RemoveBan($"{player.Id}@{player.Authentication}", BanHandler.BanType.IP);
                    BanCollection.Delete(ban.Id);
                    break;
                case PunishType.Kick:
                    var kick = KickCollection.FindOne(k => k.Target == player && k.KickId == id && k.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(kick), player, ActionType.Remove);
                    KickCollection.Delete(kick.Id);
                    break;
                case PunishType.Mute:
                    var mute = MuteCollection.FindOne(m => m.Target == player && m.MuteId == id && m.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(mute), player, ActionType.Remove);
                    if (player.IsMuted())
                        MuteHandler.RevokePersistentMute($"{player.Id}@{player.Authentication}");
                    MuteCollection.Delete(mute.Id);
                    break;
                case PunishType.Warn:
                    var warn = WarnCollection.FindOne(w => w.Target == player && w.WarnId == id && w.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(warn), player, ActionType.Remove);
                    WarnCollection.Delete(warn.Id);
                    break;
                case PunishType.SoftWarn:
                    var softWarn = SoftWarnCollection.FindOne(sw =>
                        sw.Target == player && sw.SoftWarnId == id && sw.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(softWarn), player, ActionType.Remove);
                    SoftWarnCollection.Delete(softWarn.Id);
                    break;
                case PunishType.SoftBan:
                    var softBan = SoftBanCollection.FindOne(sb =>
                        sb.Target == player && sb.SoftBanId == id && sb.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(softBan), player, ActionType.Remove);
                    SoftBanCollection.Delete(softBan.Id);
                    break;
                case PunishType.WatchList:
                    var watchList = WatchListCollection.FindOne(wl =>
                        wl.Target == player && wl.WatchListId == id && wl.Server == server);
                    JsonManager.PunishToCache(type, JsonConvert.SerializeObject(watchList), player, ActionType.Remove);
                    WatchListCollection.Delete(watchList.Id);
                    break;
            }
        }

    internal static bool CheckId(Collections.Player player, PunishType? type, int id, int server)
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

        internal static DateTime? ConvertToDateTime(string stringDuration)
        {
            var duration = Convert.ToDateTime(stringDuration).ToString("HH:mm:ss");
            return !DateTime.TryParse(duration, out var dateTime) ? (DateTime?)null : dateTime;
        }

        internal static string GetWatchList([CanBeNull] Collections.Player player)
        {
            var text = StringBuilderPool.Shared.Rent().AppendLine();
            if (player == null)
            {
                text.AppendLine($"WatchList ({WatchListCollection.Count()})").AppendLine();
                foreach (var wl in WatchListCollection.FindAll().ToList())
                {
                    text.AppendLine($"Target: {wl.Target.Name} ({wl.Target.Id}@{wl.Target.Authentication})")
                        .AppendLine($"Issuer: {wl.Issuer.Name} ({wl.Issuer.Id}@{wl.Issuer.Authentication})")
                        .AppendLine($"Reason: {wl.Reason}").AppendLine($"ID: {wl.WatchListId}")
                        .AppendLine($"Date: {wl.Date}").AppendLine($"Server sender Port: {wl.Server}").AppendLine();
                }

                return StringBuilderPool.Shared.ToStringReturn(text);
            }
            text.AppendLine($"WatchList ({player.Name} - {player.Id}@{player.Authentication})").AppendLine();
            
            foreach (var wl in WatchListCollection.Find(wl => wl.Target.Id == player.Id).ToList())
            {
                text.AppendLine($"Target: {wl.Target.Name} ({wl.Target.Id}@{wl.Target.Authentication})")
                    .AppendLine($"Issuer: {wl.Issuer.Name} ({wl.Issuer.Id}@{wl.Issuer.Authentication})")
                    .AppendLine($"Reason: {wl.Reason}").AppendLine($"ID: {wl.WatchListId}")
                    .AppendLine($"Date: {wl.Date}").AppendLine($"Server sender Port: {wl.Server}").AppendLine();
            }

            return StringBuilderPool.Shared.ToStringReturn(text);
        }
    }
}
