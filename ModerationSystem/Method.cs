using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using ModerationSystem.Collections;
using ModerationSystem.Webhook;
using static ModerationSystem.Database;
using Player = Exiled.API.Features.Player;

namespace ModerationSystem
{
    public static class Method
    {
        //Public methods
        public static void Warn(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason)
        {
            var warnid = WarnCount(LiteDatabase.GetCollection<Warn>().Find(w => w.Target.Id == dPlayer.Id).ToList());
            new Warn(dPlayer, issuer, reason, DateTime.Now, warnid).Save();
            target?.Broadcast(Plugin.Singleton.Config.WarnMessage.Duration,
                Plugin.Singleton.Config.WarnMessage.Content.Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(
                    Plugin.Singleton.Config.WarnedMessageWebHook
                        .Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}")
                        .Replace("{target.Name}", dPlayer.Name)
                        .Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{reason}", reason)
                        .Replace("{warnid}", warnid.ToString()), "**New Warn**");
        }

        public static void Mute(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason, double duration, string durationType)
        {
            var muteid = MuteCount(LiteDatabase.GetCollection<Mute>().Find(m => m.Target == dPlayer).ToList());
            new Mute(dPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddSeconds(duration), muteid).Save();
            var time = Convert.ToInt32(duration);
            Timing.RunCoroutine(MutePlayer(time, target));
            var visualDuration = "";
            switch (durationType)
            {
                case "s":
                    visualDuration = $"{duration} seconds";
                    break;
                case "m":
                    visualDuration = $"{duration / 60} minutes";
                    break;
                case "h":
                    visualDuration = $"{duration / 3600} hours";
                    break;
                case "d":
                    visualDuration = $"{duration / 86400} days";
                    break;
            }
            target?.Broadcast(Plugin.Singleton.Config.MuteMessage.Duration,
                Plugin.Singleton.Config.MuteMessage.Content.Replace("{duration}", visualDuration)
                    .Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(
                    Plugin.Singleton.Config.MutedMessageWebHook.Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}")
                        .Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@")
                        .Replace("{duration}", visualDuration).Replace("{reason}", reason)
                        .Replace("{muteid}", muteid.ToString()), "New Mute");
        }

        public static void Kick(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason)
        {
            var kickid = KickCount(LiteDatabase.GetCollection<Kick>().Find(x => x.Target.Id == dPlayer.Id).ToList());
            new Kick(dPlayer, issuer, reason, DateTime.Now, kickid).Save();
            target?.Disconnect(Plugin.Singleton.Config.KickMessage.Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(
                    Plugin.Singleton.Config.KickedMessageWebHook
                        .Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}")
                        .Replace("{target.Name}", dPlayer.Name)
                        .Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{reason}", reason)
                        .Replace("{kickid}", kickid.ToString()), "New Kick!");
        }

        public static void Ban(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason, int duration)
        {
            var banid = BanCount(LiteDatabase.GetCollection<Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList());
            new Ban(dPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddSeconds(duration), banid).Save();
            BanHandler.IssueBan(new BanDetails
            {
                Expires = DateTime.UtcNow.AddSeconds(duration).Ticks,
                Id = dPlayer.Id + "@" + dPlayer.Authentication,
                IssuanceTime = DateTime.Now.Ticks,
                Reason = reason,
                Issuer = issuer.Name,
                OriginalName = dPlayer.Name
            }, BanHandler.BanType.UserId);
            target?.Disconnect(Plugin.Singleton.Config.BanMessage.Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(
                    Plugin.Singleton.Config.BanMessageWebHook
                        .Replace("{staffer}", $"{issuer.Name} {issuer.Id}{issuer.Authentication}")
                        .Replace("{target.Name}", dPlayer.Name)
                        .Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication)
                        .Replace("{duration}", duration.ToString()).Replace("{reason}", reason)
                        .Replace("{banid}", banid.ToString()), "New Ban!");
        }

        // Private Methods
        private static IEnumerator<float> MutePlayer(float duration, Player player)
        {
            player.IsMuted = true;
            player.IsIntercomMuted = true;
            yield return Timing.WaitForSeconds(duration);
            player.IsMuted = false;
            player.IsIntercomMuted = false;
        }

        private static int BanCount(List<Ban> bans) => bans.Count;

        private static int KickCount(List<Kick> kicks) => kicks.Count;

        private static int MuteCount(List<Mute> mutes) => mutes.Count;

        private static int WarnCount(List<Warn> warns) => warns.Count;
    }
}