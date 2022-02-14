using System.Collections.Generic;
using System.Globalization;
using MEC;

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

    public class Warn : ICommand
    {
        public static Warn Instance { get; } = new Warn();

        public string Description { get; } = "Warn a player";

        public string Command { get; } = "warn";

        public string[] Aliases { get; } = { "w" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            WarnTranslation warnTranslation = Plugin.Singleton.Config.Translation.WarnTranslation;

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

            List<Collections.Player> targets = new();

            if (arguments.At(0).Split(',').Length > 1)
            {
                foreach (var player in arguments.At(0).Split(','))
                {
                    Collections.Player target = player.GetPlayer();
                    if (target is null)
                    {
                        response = warnTranslation.PlayerNotFound.Replace("{target}", player);
                        return false;
                    }

                    if (targets.Contains(target))
                    {
                        continue;
                    }
                    targets.Add(target);
                }
            }
            else
            {
                Collections.Player dPlayer = arguments.At(0).GetPlayer();
                if (dPlayer == null)
                {
                    response = warnTranslation.PlayerNotFound.Replace("{player}", arguments.At(0));
                    return false;
                }

                if (!targets.Contains(dPlayer))
                    targets.Add(dPlayer);
            }

            string reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = warnTranslation.ReasonNull;
                return false;
            }

            foreach (var target in targets)
            {
                ModerationSystemAPI.ApplyPunish(Player.Get($"{target.Id}@{target.Authentication}"), ((CommandSender)sender).GetStaffer(), target, PunishType.Warn, reason, DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
                ModerationSystemAPI.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffWarnMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{target.Name} {target.Id}{target.Authentication}").Replace("{reason}", reason)));
            }

            response = warnTranslation.PlayerWarned;
            return true;
        }
    }
}