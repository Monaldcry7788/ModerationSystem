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

    public class Kick : ICommand
    {
        public static Kick Instance { get; } = new Kick();

        public string Description { get; } = "Kick a player";

        public string Command { get; } = "kick";

        public string[] Aliases { get; } = { "k" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            KickTranslation kickTranslation = Plugin.Singleton.Config.Translation.KickTranslation;

            if (!sender.CheckPermission("ms.kick"))
            {
                response = kickTranslation.InvalidPermission.Replace("{permission}", "ms.kick");
                return false;
            }

            if (arguments.Count < 1)
            {
                response = kickTranslation.WrongUsage;
                return false;
            }

            HashSet<Collections.Player> targets = new();

            if (arguments.At(0).Split(',').Length > 1)
            {
                foreach (var player in arguments.At(0).Split(','))
                {
                    Collections.Player target = player.GetPlayer();
                    if (target is null)
                    {
                        response = kickTranslation.PlayerNotFound.Replace("{target}", player);
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
                    response = kickTranslation.PlayerNotFound.Replace("{player}", arguments.At(0));
                    return false;
                }

                if (!targets.Contains(dPlayer))
                    targets.Add(dPlayer);
            }

            string reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = kickTranslation.ReasonNull;
                return false;
            }

            foreach (var dPlayer in targets)
            {
                ModerationSystemAPI.ApplyPunish(Player.Get($"{dPlayer.Id}@{dPlayer.Authentication}"), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.Kick, reason, DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
                ModerationSystemAPI.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffKickMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason)));
                response = kickTranslation.PlayerKicked.Replace("{player.name}", $"{dPlayer.Name}").Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{reason}", reason);
            }
            response = "";
            return true;
        }
    }
}