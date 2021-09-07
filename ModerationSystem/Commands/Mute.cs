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
            Method.Mute(target, sender, issuer, dPlayer, reason, duration);
            response = $"The player {dPlayer.Name} ({dPlayer.Name}@{dPlayer.Authentication}) has been muted for: {duration} minute(s) with reason: {reason}";
            return true;
        }

    }
}
