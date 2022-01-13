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

    public class SoftBan : ICommand
    {
        public static SoftBan Instance { get; } = new SoftBan();

        public string Description { get; } = "Softban a player";

        public string Command { get; } = "softban";

        public string[] Aliases { get; } = { "sb" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            SoftBanTranslation softBanTranslation = Plugin.Singleton.Config.Translation.SoftBanTranslation;

            if (!sender.CheckPermission("ms.softban"))
            {
                response = softBanTranslation.InvalidPermission.Replace("{permission}", "ms.softban");
                return false;
            }

            if (arguments.Count < 2)
            {
                response = softBanTranslation.WrongUsage;
                return false;
            }

            Collections.Player dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = softBanTranslation.PlayerNotFound;
                return false;
            }

            DateTime? duration = ModerationSystemAPI.ConvertToDateTime(arguments.At(1));
            if (duration == null)
            {
                response = softBanTranslation.InvalidDuration.Replace("{duration}", arguments.At(1));
                return false;
            }
            
            string reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = softBanTranslation.ReasonNull;
                return false;
            }

            if (dPlayer.IsSoftBanned())
            {
                response = softBanTranslation.PlayerAlreadySoftBanned;
                return false;
            }

            ModerationSystemAPI.ApplyPunish(Player.Get(arguments.At(0)), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.SoftBan, reason, Convert.ToDateTime(duration));
            ModerationSystemAPI.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffWarnMessage.Content
                .Replace("{staffer}", sender.LogName)
                .Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}")
                .Replace("{reason}", reason)));
            response = softBanTranslation.PlayerSoftBanned.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{duration}", duration.ToString()).Replace("{reason}", reason);
            return true;
        }
    }
}