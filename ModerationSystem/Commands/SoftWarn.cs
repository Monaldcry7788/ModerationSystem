using System.Collections.Generic;
using System.Globalization;

namespace ModerationSystem.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using ModerationSystem.Configs.CommandTranslation;
    using ModerationSystem.Enums;
    using API;

    public class SoftWarn : ICommand
    {
        public static SoftWarn Instance { get; } = new SoftWarn();

        public string Description { get; } = "Softwarn a player";

        public string Command { get; } = "softwarn";

        public string[] Aliases { get; } = { "sw" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            SoftWarnTranslation softWarnTranslation = Plugin.Singleton.Config.Translation.SoftWarnTranslation;

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
            HashSet<Collections.Player> targets = new();

            if (arguments.At(0).Split(',').Length > 1)
            {
                foreach (string player in arguments.At(0).Split(','))
                {
                    Collections.Player target = player.GetPlayer();
                    if (target is null)
                    {
                        response = softWarnTranslation.PlayerNotFound.Replace("{target}", player);
                        continue;
                    }

                    if (targets.Contains(target)) continue;
                    targets.Add(target);
                }
            }
            else
            {
                Collections.Player dPlayer = arguments.At(0).GetPlayer();
                if (dPlayer == null)
                {
                    response = softWarnTranslation.PlayerNotFound.Replace("{player}", arguments.At(0));
                    return false;
                }

                if (!targets.Contains(dPlayer))
                    targets.Add(dPlayer);
            }

            string reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = softWarnTranslation.ReasonNull;
                return false;
            }

            foreach (Collections.Player target in targets)
            {
                ModerationSystemAPI.ApplyPunish(Player.Get($"{target.Id}@{target.Authentication}"), ((CommandSender)sender).GetStaffer(), target, PunishType.SoftWarn, reason, DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
                ModerationSystemAPI.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffWarnMessage.Content
                    .Replace("{staffer}", sender.LogName)
                    .Replace("{target}", $"{target.Name} {target.Id}{target.Authentication}")
                    .Replace("{reason}", reason)));
                response = softWarnTranslation.PlayerSoftWarned.Replace("{player.name}", target.Name)
                    .Replace("{player.userid}", $"{target.Id}@{target.Authentication}").Replace("{reason}", reason);
            }
            response = "";
            return true;
        }
    }
}