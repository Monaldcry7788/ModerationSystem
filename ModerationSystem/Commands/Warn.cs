using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Broadcast = Exiled.API.Features.Broadcast;

namespace ModerationSystem.Commands
{
    public class Warn : ICommand
    {
        private Warn()
        {
        }

        public static Warn Instance { get; } = new Warn();

        public string Description { get; } = "Warn a player";

        public string Command { get; } = "warn";

        public string[] Aliases { get; } = { "w" };

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

            Method.Warn(target, issuer, dPlayer, reason);
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.StaffWarnMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason)));
            response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been warned for: {reason}";
            return true;
        }
    }
}