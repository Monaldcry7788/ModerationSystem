﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
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
            var i = WarnCount(LiteDatabase.GetCollection<Warn>().Find(w => w.Target.Id == dPlayer.Id).ToList());
            var warnid = Convert.ToInt32(i);
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

        public static void Mute(Player target, ICommandSender sender, Collections.Player issuer,
            Collections.Player dPlayer, string reason, double duration)
        {
            var i = MuteCount(LiteDatabase.GetCollection<Mute>().Find(w => w.Target.Id == dPlayer.Id).ToList());
            var muteid = Convert.ToInt32(i);
            new Mute(dPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddMinutes(duration), muteid).Save();
            var time = Convert.ToInt32(duration);
            Timing.RunCoroutine(MutePlayer(time * 60, target));
            target?.Broadcast(Plugin.Singleton.Config.MuteMessage.Duration,
                Plugin.Singleton.Config.MuteMessage.Content.Replace("{duration}", duration.ToString())
                    .Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
                Http.sendMessage(
                    Plugin.Singleton.Config.MutedMessageWebHook.Replace("{staffer}", sender.LogName)
                        .Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@")
                        .Replace("{duration}", duration.ToString()).Replace("{reason}", reason)
                        .Replace("{muteid}", muteid.ToString()), "New Mute");
        }

        public static void Kick(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason)
        {
            var i = KickCount(LiteDatabase.GetCollection<Kick>().Find(x => x.Target.Id == dPlayer.Id).ToList());
            var kickid = Convert.ToInt32(i);
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

        public static void Ban(Player target, Collections.Player issuer, Collections.Player dPlayer, string reason,
            int duration)
        {
            var i = BanCount(LiteDatabase.GetCollection<Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList());
            var banid = Convert.ToInt32(i);
            new Ban(dPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddMinutes(duration), banid).Save();
            BanHandler.IssueBan(new BanDetails
            {
                Expires = DateTime.UtcNow.AddMinutes(duration).Ticks,
                Id = dPlayer.Id + "@" + dPlayer.Authentication,
                IssuanceTime = DateTime.Now.Ticks,
                Reason = reason,
                Issuer = issuer.Name,
                OriginalName = dPlayer.Name
            }, BanHandler.BanType.UserId);
            target?.Disconnect($"You has been banned!: {reason}");
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

        private static string BanCount(List<Ban> bans)
        {
            bans.Count().ToString();
            return bans.Count().ToString();
        }

        private static string KickCount(List<Kick> kicks)
        {
            kicks.Count().ToString();
            return kicks.Count().ToString();
        }

        private static string MuteCount(List<Mute> mutes)
        {
            mutes.Count().ToString();
            return mutes.Count().ToString();
        }

        private static string WarnCount(List<Warn> warns)
        {
            warns.Count().ToString();
            return warns.Count().ToString();
        }
    }
}