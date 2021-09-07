namespace ModerationSystem.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Database;
    public class Warn : ICommand
    {
        private Warn()
        {
        }

        public static Warn Instance { get; } = new Warn();

        public string Description { get; } = "Warn a player";

        public string Command { get; } = "warn";

        public string[] Aliases { get; } = new[] { "w" };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.warn"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: ms warn/w <player name or ID> <reason>";
                return false;
            }
            Collections.Player dPlayer = arguments.At(0).GetPlayer();
            Collections.Player issuer = ((CommandSender)sender).GetStaffer();
            Player target = Player.Get(arguments.At(0));
            if (dPlayer == null)
            {
                response = "Player not found!";
                return false;
            }
            string reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
                return false;
            }
            Method.Warn(target, sender, issuer, dPlayer, reason);
            response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been warned for: {reason}";
            return true;

        }
    }
}
