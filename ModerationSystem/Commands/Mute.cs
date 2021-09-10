using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ModerationSystem.Commands
{
    public class Mute : ICommand
    {
        private Mute()
        {
        }

        public static Mute Instance { get; } = new Mute();

        public string Command { get; } = "mute";

        public string[] Aliases { get; } = { "m" };

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
                response = "Usage: ms mute/m <player name or ID> <time(s/m/h/d)> <reason>";
                return false;
            }

            var target = Player.Get(arguments.At(0));
            var dPlayer = arguments.At(0).GetPlayer();
            var issuer = ((CommandSender)sender).GetStaffer();

            if (dPlayer == null)
            {
                response = "Player not found!!";
                return false;
            }
            
            string duration;
            try
            {
                duration = Convert.ToDateTime(arguments.At(1)).ToString("HH:mm:ss");
                if (!DateTime.TryParse(duration, out DateTime durationDateTime))
                {
                    response = $"{durationDateTime} is not a valid duration. Valid id: ss:mm:hh:dd";
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

            if (dPlayer.IsMuted())
            {
                response = "Player already muted";
                return false;
            }
            Method.Mute(target, issuer, dPlayer, reason, Convert.ToDateTime(duration));
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.StaffMuteMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason).Replace("{time}", duration)));
            response = $"The player {dPlayer.Name} ({dPlayer.Name}@{dPlayer.Authentication}) has been muted for: {duration}. With reason: {reason}";
            return true;
        }
    }
}