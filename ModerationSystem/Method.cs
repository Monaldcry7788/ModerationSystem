using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exiled.API.Features;
using MEC;
using ModerationSystem.Collections;
using static ModerationSystem.Database;
using Player = Exiled.API.Features.Player;

namespace ModerationSystem
{
    public static class Method
    {
        public static void Warn(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason)
        {
            new Warn(dPlayer, issuer, reason, DateTime.Now, LiteDatabase.GetCollection<Warn>().Find(w => w.Target.Id == dPlayer.Id).ToList().Count).Save();
            target?.Broadcast(Plugin.Singleton.Config.PlayerWarnMessage.Duration,
                Plugin.Singleton.Config.PlayerWarnMessage.Content.Replace("{reason}", reason));
        }

        public static void Mute(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason, DateTime duration)
        {
            new Mute(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration)), LiteDatabase.GetCollection<Mute>().Find(m => m.Target == dPlayer).ToList().Count).Save();
            if (target == null) return;
            Mute(target, dPlayer);
            target.Broadcast(Plugin.Singleton.Config.PlayerMuteMessage.Duration, Plugin.Singleton.Config.PlayerMuteMessage.Content.Replace("{duration}", duration.ToString("HH:mm:ss")).Replace("{reason}", reason));
        }

        public static void Kick(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason)
        {
            new Kick(dPlayer, issuer, reason, DateTime.Now, LiteDatabase.GetCollection<Kick>().Find(x => x.Target.Id == dPlayer.Id).ToList().Count).Save();
            target?.Kick(Plugin.Singleton.Config.PlayerKickMessage.Replace("{reason}", reason));
        }

        public static void Ban(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason, DateTime duration)
        {
            new Ban(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration)), LiteDatabase.GetCollection<Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList().Count).Save();
            target?.Ban(GetTotalSeconds(duration), Plugin.Singleton.Config.PlayerBanMessage.Replace("{reason}", reason), "ModerationSystem");
        }

        public static void SendBroadcast(Exiled.API.Features.Broadcast broadcast)
        {
            foreach (var player in Player.List.Where(p => p.RemoteAdminAccess)) player.Broadcast(broadcast, true);
        }

        private static int GetTotalSeconds(DateTime time)
        {
            int totalSeconds = (time.Hour*3600)+(time.Minute*60)+time.Second;
            return totalSeconds;
        }

        private static void Mute(Player player, Collections.Player dPlayer)
        {
            player.IsMuted = true;
            player.IsIntercomMuted = true;
            dPlayer.IsActuallyMuted = true;
            dPlayer.Save();
        }
    }
}