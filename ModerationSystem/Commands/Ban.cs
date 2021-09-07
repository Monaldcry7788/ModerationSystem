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

            var reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));
            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
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
            
            if (dPlayer.IsBanned())
            {
                response = "Player is already banned!";
                return false;
            }

            switch (durationType.ToString())
            {
                case "s":
                    Method.Ban(target, issuer, dPlayer, reason, duration);
                    break;
                case "m":
                    Method.Ban(target, issuer, dPlayer, reason, duration*60);
                    break;
                case "h":
                    Method.Ban(target, issuer, dPlayer, reason, duration * 3600);
                    break;
                case "d":
                    Method.Ban(target, issuer, dPlayer, reason, duration*86400);
                    break;
            }
            response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been banned for {duration}{durationType} with reason: {reason}";
            return true;
        }
    }
}