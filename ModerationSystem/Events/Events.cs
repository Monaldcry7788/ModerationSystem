using System.IO;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using ModerationSystem.Collections;
using ModerationSystem.Enums;
using Newtonsoft.Json;
using Player = ModerationSystem.Collections.Player;

namespace ModerationSystem.Events
{
    using static Database;

    internal class Events
    {

        internal static void OnVerified(VerifiedEventArgs ev)
        {
            var dPlayer = ev.Player.GetPlayer() ?? new Collections.Player(
                ev.Player.RawUserId,
                ev.Player.AuthenticationType.ToString().ToLower(),
                ev.Player.Nickname
            );
            PlayerData.Add(ev.Player, dPlayer);

            if (dPlayer.Name != ev.Player.Nickname)
            {
                dPlayer.Name = ev.Player.Nickname;
                dPlayer.Save();
            }
            if (!dPlayer.IsMuted() && MuteHandler.QueryPersistentMute($"{dPlayer.Id}@{dPlayer.Authentication}"))
                MuteHandler.RevokePersistentMute($"{dPlayer.Id}@{dPlayer.Authentication}");
        }

        internal static void OnDestroying(DestroyingEventArgs ev)
        {
            ev.Player.GetPlayer().Save();

            PlayerData.Remove(ev.Player);
        }

        internal static void OnChangingRole(ChangingRoleEventArgs e)
        {
            if (e.Player.GetPlayer().IsSoftBanned())
                e.IsAllowed = false;
        }

        internal static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!Plugin.Singleton.Config.IsDatabaseGlobal) return;
            if (!Plugin.Singleton.Config.ReceiveFrom.Contains(Path.GetFileNameWithoutExtension(e.Name)?.Split('-')[4])) return;
            var playerId = Path.GetFileNameWithoutExtension(e.Name)?.Split('-')[2];
            var rawPlayerId = playerId?.Split('@')[0];
            if (!PlayerCollection.Exists(p => p.Id == rawPlayerId))
            {
                var player = new Player
                (
                    rawPlayerId, 
                    playerId?.Split('@')[1], 
                    e.Name?.Split('-')[3]
                );
                player.Save();
            }

            switch (Path.GetFileNameWithoutExtension(e.Name)?.Split('-')[1].GetPunishType())
            {
                case PunishType.Ban:
                    var ban = Utf8Json.JsonSerializer.Deserialize<Ban>(File.ReadAllText(e.FullPath));
                    if (ban != null && !ban.Clear)
                    {
                        new Ban(ban.Target, ban.Issuer, ban.Reason, ban.Duration, ban.Date, ban.Expire, ban.BanId,
                            ban.Server, false).Save();
                        break;
                    }

                    BanCollection.DeleteMany(b =>
                        b.Target == ban.Target && b.BanId == ban.BanId && b.Server == ban.Server);
                    break;
                case PunishType.Kick:
                    var kick = Utf8Json.JsonSerializer.Deserialize<Kick>(File.ReadAllText(e.FullPath));
                    if (kick != null && !kick.Clear)
                    {
                        new Kick(kick.Target, kick.Issuer, kick.Reason, kick.Date, kick.KickId, kick.Server,
                            false).Save();
                        break;
                    }

                    KickCollection.DeleteMany(k =>
                        k.Target == kick.Target && k.KickId == kick.KickId && k.Server == kick.Server);
                    break;
                case PunishType.Mute:
                    var mute = Utf8Json.JsonSerializer.Deserialize<Mute>(File.ReadAllText(e.FullPath));
                    if (mute != null && !mute.Clear)
                    {
                        new Mute(mute.Target, mute.Issuer, mute.Reason, mute.Duration, mute.Date, mute.Expire,
                            mute.MuteId, mute.Server, false).Save();
                        break;
                    }

                    KickCollection.DeleteMany(m =>
                        m.Target == mute.Target && m.KickId == mute.MuteId && m.Server == mute.Server);
                    break;
                case PunishType.Warn:
                    var warn = Utf8Json.JsonSerializer.Deserialize<Warn>(File.ReadAllText(e.FullPath));
                    if (warn != null && !warn.Clear)
                    {
                        new Warn(warn.Target, warn.Issuer, warn.Reason, warn.Date, warn.WarnId, warn.Server,
                            false).Save();
                        break;
                    }

                    WarnCollection.DeleteMany(w =>
                        w.Target == warn.Target && w.WarnId == warn.WarnId && w.Server == warn.Server);
                    break;
                case PunishType.SoftBan:
                    var softBan = Utf8Json.JsonSerializer.Deserialize<SoftBan>(File.ReadAllText(e.FullPath));
                    if (softBan != null && !softBan.Clear)
                    {
                        new SoftBan(softBan.Target, softBan.Issuer, softBan.Reason, softBan.Duration, softBan.Date,
                            softBan.Expire, softBan.SoftBanId, softBan.Server, false).Save();
                        break;
                    }

                    SoftBanCollection.DeleteMany(sb =>
                        sb.Target == softBan.Target && sb.SoftBanId == softBan.SoftBanId &&
                        sb.Server == softBan.Server);
                    break;
                case PunishType.SoftWarn:
                    var softWan = Utf8Json.JsonSerializer.Deserialize<SoftWarn>(File.ReadAllText(e.FullPath));
                    if (softWan != null && !softWan.Clear)
                    {
                        new SoftWarn(softWan.Target, softWan.Issuer, softWan.Reason, softWan.Date, softWan.SoftWarnId,
                            softWan.Server, false).Save();
                        break;
                    }

                    SoftWarnCollection.DeleteMany(sw =>
                        sw.Target == softWan.Target && sw.SoftWarnId == softWan.SoftWarnId &&
                        sw.Server == softWan.Server);
                    break;
                case PunishType.WatchList:
                    var watchList = JsonConvert.DeserializeObject<WatchList>(File.ReadAllText(e.FullPath));
                    if (watchList != null && !watchList.Clear)
                    {
                        new WatchList(watchList.Target, watchList.Issuer, watchList.Reason, watchList.Date,
                            watchList.WatchListId, watchList.Server, false).Save();
                        break;
                    }

                    WatchListCollection.DeleteMany(wl =>
                        wl.Target == watchList.Target && wl.WatchListId == watchList.WatchListId &&
                        wl.Server == watchList.Server);
                    break;
                case PunishType.All:
                    JsonConvert.DeserializeObject<Player>(File.ReadAllText(e.FullPath)).Clear();
                    break;
            }
            File.Delete(e.FullPath);

        }
    }
}
