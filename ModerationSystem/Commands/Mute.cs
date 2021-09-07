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
            var arg1 = arguments.At(1).ToCharArray().ToList();
            var durationType = arg1.LastOrDefault();
            arg1.Remove(arg1.LastOrDefault());
            if (durationType.ToString() != "s" && durationType.ToString() != "m" && durationType.ToString() != "h" && durationType.ToString() != "d")
            {
                response = $"{durationType} is not a valid durationType. Avariables: s/m/h/d";
                return false;
            }

            if (!int.TryParse(arguments.At(1).Replace(durationType.ToString(), ""), out var duration) || duration < 1)
            {
                response = "Insert a valid duration";
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
            switch (durationType.ToString())
            {
                case "s":
                    Method.Mute(target, issuer, dPlayer, reason, duration, durationType.ToString());
                    break;
                case "m":
                    Method.Mute(target, issuer, dPlayer, reason, duration*60, durationType.ToString());
                    break;
                case "h":
                    Method.Mute(target, issuer, dPlayer, reason, duration * 3600, durationType.ToString());
                    break;
                case "d":
                    Method.Mute(target, issuer, dPlayer, reason, duration*86400, durationType.ToString());
                    break;
            }
            response = $"The player {dPlayer.Name} ({dPlayer.Name}@{dPlayer.Authentication}) has been muted for: {duration}{durationType} with reason: {reason}";
            return true;
        }
    }
}