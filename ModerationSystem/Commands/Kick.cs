using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using ModerationSystem.Enums;

namespace ModerationSystem.Commands
{
    public class Kick : ICommand
    {
        private Kick()
        {
        }

        public static Kick Instance { get; } = new Kick();

        public string Description { get; } = "Kick a player";

        public string Command { get; } = "kick";

        public string[] Aliases { get; } = { "k" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var kickTranslation = Plugin.Singleton.Config.Translation.KickTranslation;
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

            var dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = kickTranslation.PlayerNotFound;
                return false;
            }

            var reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = kickTranslation.ReasonNull;
                return false;
            }
            
            Method.ApplyPunish(Player.Get(arguments.At(0)), ((CommandSender)sender).GetStaffer(), dPlayer, PunishType.Kick, reason, DateTime.MinValue);
            Method.SendBroadcast(new Exiled.API.Features.Broadcast(Plugin.Singleton.Config.Translation.StaffTranslation.StaffKickMessage.Content.Replace("{staffer}", sender.LogName).Replace("{target}", $"{dPlayer.Name} {dPlayer.Id}{dPlayer.Authentication}").Replace("{reason}", reason)));
            response = kickTranslation.PlayerKicked.Replace("{player.name}", $"{dPlayer.Name}").Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}").Replace("{reason}", reason);
            return true;
        }
    }
}