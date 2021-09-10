using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using ModerationSystem.Collections;
using ModerationSystem.Webhook;
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
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(Plugin.Singleton.Config.WarnedMessageWebHook.Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}").Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{reason}", reason).Replace("{warnid}", LiteDatabase.GetCollection<Warn>().Find(w => w.Target.Id == dPlayer.Id).ToList().Count.ToString()), "**New Warn**");
        }

        public static void Mute(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason, DateTime duration)
        {
            new Mute(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration)), LiteDatabase.GetCollection<Mute>().Find(m => m.Target == dPlayer).ToList().Count).Save();
            Timing.RunCoroutine(MutePlayer(GetTotalSeconds(duration), target));
            target?.Broadcast(Plugin.Singleton.Config.PlayerMuteMessage.Duration, Plugin.Singleton.Config.PlayerMuteMessage.Content.Replace("{duration}", duration.ToString("HH:mm:ss")).Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(Plugin.Singleton.Config.MutedMessageWebHook.Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}").Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@").Replace("{duration}", duration.ToString("HH:mm:ss")).Replace("{reason}", reason).Replace("{muteid}", LiteDatabase.GetCollection<Mute>().Find(m => m.Target == dPlayer).ToList().Count.ToString()), "New Mute");
        }

        public static void Kick(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason)
        {
            new Kick(dPlayer, issuer, reason, DateTime.Now, LiteDatabase.GetCollection<Kick>().Find(x => x.Target.Id == dPlayer.Id).ToList().Count).Save();
            target?.Kick(Plugin.Singleton.Config.PlayerKickMessage.Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled) Http.sendMessage(Plugin.Singleton.Config.KickedMessageWebHook.Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}").Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{reason}", reason).Replace("{kickid}", LiteDatabase.GetCollection<Kick>().Find(x => x.Target.Id == dPlayer.Id).ToList().Count.ToString()), "New Kick!");
        }

        public static void Ban(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason, DateTime duration)
        {
            new Ban(dPlayer, issuer, reason, duration.ToString("HH:mm:ss"), DateTime.Now, DateTime.Now.AddSeconds(GetTotalSeconds(duration)), LiteDatabase.GetCollection<Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList().Count).Save();
            target?.Ban(GetTotalSeconds(duration), Plugin.Singleton.Config.PlayerBanMessage.Replace("{reason}", reason), "ModerationSystem");
            if (Plugin.Singleton.WebhookEnabled) Http.sendMessage(Plugin.Singleton.Config.BanMessageWebHook.Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}").Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{duration}", duration.ToString()).Replace("{reason}", reason).Replace("{banid}", LiteDatabase.GetCollection<Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList().Count.ToString()), "New Ban!");
        }

        public static void SendBroadcast(Exiled.API.Features.Broadcast broadcast)
        {
            foreach (var player in Player.List.Where(p => p.RemoteAdminAccess)) player.Broadcast(broadcast, true);
        }

        private static IEnumerator<float> MutePlayer(int duration, Player player)
        {
            player.IsMuted = true;
            player.IsIntercomMuted = true;
            yield return Timing.WaitForSeconds(duration);
            player.IsMuted = false;
            player.IsIntercomMuted = false;
        }

        private static int GetTotalSeconds(DateTime time)
        {
            int totalSeconds = (time.Hour*3600)+(time.Minute*60)+time.Second;
            return totalSeconds;
        }
    }
}