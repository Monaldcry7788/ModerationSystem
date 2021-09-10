using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ModerationSystem.Commands
{
    public class Ban : ICommand
    {
        private Ban()
        {
        }

        public static Ban Instance { get; } = new Ban();

        public string Description { get; } = "Ban a player";

        public string Command { get; } = "ban";

        public string[] Aliases { get; } = { "b" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.ban"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: ms ban/b <player name or ID> <duration(s/m/h/d)> <reason>";
                return false;
            }

            var dPlayer = arguments.At(0).GetPlayer();
            var issuer = ((CommandSender)sender).GetStaffer();
            var target = Player.Get(arguments.At(0));
            if (dPlayer == null)
            {
                response = "Player not found!";
                return false;
            }

            string duration;
            try
            {
                duration = Convert.ToDateTime(arguments.At(1)).ToString("HH:mm:ss");
                if (!DateTime.TryParse(duration, out DateTime durationDateTime))
                {
                    response = $"{durationDateTime} is not a valid duration. Valid id: HH:mm:ss";
                    return false;
                }
            }
            catch (Exception e)
            {
                response = $"{arguments.At(1)} is not a valid duration. Valid duration: HH:mm:ss";
                return false;
            }

            var reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));
            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
                return false;
            }

            if (dPlayer.IsBanned())
            {
                response = "Player is already banned!";
                return false;
            }
            
            Method.Ban(target, issuer, dPlayer, reason, Convert.ToDateTime(duration));
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.StaffBanMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason).Replace("{time}", duration)));
            response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been banned for {duration}. With reason: {reason}";
            return true;
        }
    }
}