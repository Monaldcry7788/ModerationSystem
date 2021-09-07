using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace ModerationSystem.Commands
{
    public class Kick : ICommand
    {
        private Kick()
        {
        }

        public static Kick Instance { get; } = new Kick();

        public string Description { get; } = "Kick a player";

        public string Command { get; } = "kick";

        public string[] Aliases { get; } = { "k" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.kick"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: ms kick/k <player name or ID> <reason>";
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

            var reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
                return false;
            }

            var players = Player.List.Where(x => x.RawUserId == dPlayer.Id).ToList();
            if (!players.IsEmpty())
            {
                Method.Kick(target, issuer, dPlayer, reason);
                response =
                    $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been kicked for: {reason}";
                return true;
            }

            response = "Player not found!";
            return false;
        }
    }
}