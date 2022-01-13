﻿namespace ModerationSystem.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using ModerationSystem.Enums;
    using ModerationSystem.Configs.CommandTranslation;
    using API;

    public class Ban : ICommand
    {
        public static Ban Instance { get; } = new Ban();

        public string Description { get; } = "Ban a player";

        public string Command { get; } = "ban";

        public string[] Aliases { get; } = { "b" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            BanTranslation banTranslation = Plugin.Singleton.Config.Translation.BanTranslation;
 
            if (!sender.CheckPermission("ms.ban"))
            {
                response = banTranslation.InvalidPermission.Replace("{permission}", "ms.ban");
                return false;
            }

            if (arguments.Count < 2)
            {
                response = banTranslation.WrongUsage;
                return false;
            }

            Collections.Player dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = banTranslation.PlayerNotFound;
                return false;
            }

            DateTime? duration = ModerationSystemAPI.ConvertToDateTime(arguments.At(1));
            if (duration == null)
            {
                response = banTranslation.InvalidDuration.Replace("{duration}", arguments.At(1));
                return false;
            }
 
            string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));
            if (string.IsNullOrEmpty(reason))
            {
                response = banTranslation.ReasonNull;
                return false;
            }

            if (dPlayer.IsBanned())
            {
                response = banTranslation.PlayerAlreadyBanned;
                return false;
            }

            ModerationSystemAPI.ApplyPunish(Player.Get(arguments.At(0)), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.Ban, reason, Convert.ToDateTime(duration));
            ModerationSystemAPI.SendBroadcast(new Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffBanMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason).Replace("{time}", duration.ToString())));
            response = banTranslation.PlayerBanned.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{duration}", duration.ToString()).Replace("{reason}", reason);
            return true;
        }
    }
}