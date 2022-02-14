using System.Collections.Generic;

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

    public class Mute : ICommand
    {
        public static Mute Instance { get; } = new Mute();

        public string Command { get; } = "mute";

        public string[] Aliases { get; } = { "m" };

        public string Description { get; } = "Mute a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            MuteTranslation muteTranslation = Plugin.Singleton.Config.Translation.MuteTranslation;

            if (!sender.CheckPermission("ms.mute"))
            {
                response = muteTranslation.InvalidPermission.Replace("{permission}", "ms.mute");
                return false;
            }

            if (arguments.Count < 2)
            {
                response = muteTranslation.WrongUsage;
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
                        response = muteTranslation.PlayerNotFound.Replace("{target}", player);
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
                    response = muteTranslation.PlayerNotFound.Replace("{player}", arguments.At(0));
                    return false;
                }

                if (!targets.Contains(dPlayer))
                    targets.Add(dPlayer);
            }

            DateTime? duration = ModerationSystemAPI.ConvertToDateTime(arguments.At(1));
            if (duration == null)
            {
                response = muteTranslation.InvalidDuration.Replace("{duration}", arguments.At(1));
                return false;
            }

            string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));
            if (string.IsNullOrEmpty(reason))
            {
                response = muteTranslation.ReasonNull;
                return false;
            }

            if (!ModerationSystemAPI.MaxDuration(arguments.At(1), Player.Get(sender)))
            {
                response = "You can't do this duration";
                return false;
            }

            foreach (var dPlayer in targets)
            {
                if (dPlayer.IsMuted())
                {
                    response = muteTranslation.PlayerAlreadyMuted;
                    return false;
                }

                ModerationSystemAPI.ApplyPunish(Player.Get($"{dPlayer.Id}@{dPlayer.Authentication}"), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.Mute, reason, arguments.At(1));
                ModerationSystemAPI.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffMuteMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason).Replace("{time}", duration.ToString())));
                response = muteTranslation.PlayerMuted.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{duration}", duration.ToString()).Replace("{reason}", reason);
            }
            response = "";
            return true;
        }
    }
}