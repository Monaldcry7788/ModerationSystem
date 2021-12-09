using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using ModerationSystem.Enums;

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
            var warnTranslation = Plugin.Singleton.Config.Translation.WarnTranslation;
            if (!sender.CheckPermission("ms.warn"))
            {
                response = warnTranslation.InvalidPermission.Replace("{permission}", "ms.warn");
                return false;
            }

            if (arguments.Count < 1)
            {
                response = warnTranslation.WrongUsage;
                return false;
            }

            var dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = warnTranslation.PlayerNotFound;
                return false;
            }

            var reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = warnTranslation.ReasonNull;
                return false;
            }

            Method.ApplyPunish(Player.Get(arguments.At(0)), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.Warn, reason, DateTime.MinValue);
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffWarnMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason)));
            response = warnTranslation.PlayerWarned.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{reason}", reason);
            return true;
        }
    }
}