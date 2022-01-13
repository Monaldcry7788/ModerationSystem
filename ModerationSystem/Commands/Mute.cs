namespace ModerationSystem.Commands
{
    using System;
    using System.Linq;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using ModerationSystem.Configs.CommandTranslation;
    using ModerationSystem.Enums;

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

            Collections.Player dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = muteTranslation.PlayerNotFound;
                return false;
            }

            DateTime? duration = Method.ConvertToDateTime(arguments.At(1));
            if (duration == null)
            {
                response = muteTranslation.InvalidDuration.Replace("{duration}", arguments.At(1));
            }

            string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));
            if (string.IsNullOrEmpty(reason))
            {
                response = muteTranslation.ReasonNull;
                return false;
            }

            if (dPlayer.IsMuted())
            {
                response = muteTranslation.PlayerAlreadyMuted;
                return false;
            }

            Method.ApplyPunish(Player.Get(arguments.At(0)), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.Mute, reason, Convert.ToDateTime(duration));
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffMuteMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason).Replace("{time}", duration.ToString())));
            response = muteTranslation.PlayerMuted.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{duration}", duration.ToString()).Replace("{reason}", reason);
            return true;
        }
    }
}