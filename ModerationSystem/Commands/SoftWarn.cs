using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using ModerationSystem.Enums;

namespace ModerationSystem.Commands
{
    public class SoftWarn : ICommand
    {
        private SoftWarn()
        {
        }

        public static SoftWarn Instance { get; } = new SoftWarn();

        public string Description { get; } = "Softwarn a player";

        public string Command { get; } = "softwarn";

        public string[] Aliases { get; } = { "sw" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var softWarnTranslation = Plugin.Singleton.Config.Translation.SoftWarnTranslation;
            if (!sender.CheckPermission("ms.softwarn"))
            {
                response = softWarnTranslation.InvalidPermission.Replace("{permission}", "ms.softwarn");
                return false;
            }

            if (arguments.Count < 1)
            {
                response = softWarnTranslation.WrongUsage;
                return false;
            }

            var dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = softWarnTranslation.PlayerNotFound;
                return false;
            }

            var reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = softWarnTranslation.ReasonNull;
                return false;
            }

            Method.ApplyPunish(Player.Get(arguments.At(0)), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.SoftWarn, reason, DateTime.MinValue);
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffWarnMessage.Content
                .Replace("{staffer}", sender.LogName)
                .Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}")
                .Replace("{reason}", reason)));
            response = softWarnTranslation.PlayerSoftWarned.Replace("{player.name}", dPlayer.Name)
                .Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{reason}", reason);
            return true;
        }
    }
}