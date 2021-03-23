namespace ModerationSystem.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using MEC;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static ModerationSystem.Database;

    public class Mute : ICommand
    {
        private Mute()
        {
        }

        public static Mute Instance { get; } = new Mute();

        public string Command { get; } = "mute";

        public string[] Aliases { get; } = new[] { "m" };

        public string Description { get; } = "Mute a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.mute"))
            {
                response = "You can't do this command!";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: ms mute/m <player name or ID> <time (in minutes)> <reason>";
                return false;
            }

            Player target = Player.Get(arguments.At(0));
            Collections.Player dPlayer = arguments.At(0).GetPlayer();
            Collections.Player issuer = ((CommandSender)sender).GetStaffer();

            if (dPlayer == null)
            {
                response = "Player not found!!";
                return false;
            }

            if (!double.TryParse(arguments.At(1), out double duration) || duration < 1)
            {
                response = "Insert a valid duration";
                return false;
            }

            string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));

            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
                return false;
            }

            if (dPlayer.IsMuted())
            {
                response = "Player already muted";
                return false;
            }
            var i = MuteCount(LiteDatabase.GetCollection<Collections.Mute>().Find(w => w.Target.Id == dPlayer.Id).ToList());
            var muteid = Convert.ToInt32(i);
            new Collections.Mute(dPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddMinutes(duration), muteid).Save();
            int time = Convert.ToInt32(duration);
            MutePlayer(time * 60, target);
            target?.Broadcast(Plugin.Singleton.Config.MuteMessage.Duration, Plugin.Singleton.Config.MuteMessage.Content.Replace("{duration}", duration.ToString()).Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
            {
                Webhook.Http.sendMessage(Plugin.Singleton.Config.MutedMessageWebHook.Replace("{staffer}", sender.LogName).Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@").Replace("{duration}", duration.ToString()).Replace("{reason}", reason).Replace("{muteid}", muteid.ToString()), "New Mute");
            }
            response = $"The player {dPlayer.Name} ({dPlayer.Name}@{dPlayer.Authentication}) has been muted for: {duration} minute(s) with reason: {reason}";
            return true;
        }

        private IEnumerator<float> MutePlayer(float duration, Exiled.API.Features.Player player)
        {
            player.IsMuted = true;
            player.IsIntercomMuted = true;
            yield return Timing.WaitForSeconds(duration);
            player.IsMuted = false;
            player.IsIntercomMuted = false;
        }
        private string MuteCount(List<Collections.Mute> mutes)
        {
            mutes.Count().ToString();
            return mutes.Count().ToString();
        }
    }
}
