using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Log = Exiled.API.Features.Log;

namespace ModerationSystem.Commands
{
    public class Clear : ICommand
    {
        private Clear()
        {
        }

        public static Clear Instance { get; } = new Clear();

        public string Description { get; } = "Clear a player";

        public string Command { get; } = "clear";

        public string[] Aliases { get; } = { "c" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var clearTranslation = Plugin.Singleton.Config.Translation.ClearTranslation;
            if (!sender.CheckPermission("ms.clear"))
            {
                response = clearTranslation.InvalidPermission.Replace("{permission}", "ms.clear");
                return false;
            }
            if (arguments.Count == 0 || arguments.Count > 4 || arguments.Count == 3 || arguments.Count == 1 || (arguments.Count == 2 && !arguments.At(1).Equals("all")))
            {
                response = clearTranslation.WrongUsage;
                return false;
            }

            var dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = clearTranslation.PlayerNotFound;
                return false;
            }

            switch (arguments.At(1))
            {
                case "all":
                    dPlayer.Clear();
                    response = clearTranslation.PlayerCleared.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}");
                    return true;
                case "ban":
                case "kick":
                case "mute":
                case "warn":
                case "softban":
                case "softwarn":
                case "watchlist":
                    var action = arguments.At(1).GetPunishType();
                    if (!int.TryParse(arguments.At(2), out var id))
                    {
                        response = clearTranslation.IdNotFound;
                        return false;
                    }

                    if (!int.TryParse(arguments.At(3), out var server))
                    {
                        response = "Server port not found";
                        return false;
                    }

                    if (!Method.CheckId(dPlayer, action, id, server))
                    {
                        response = clearTranslation.IdNotFound;
                        return false;
                    }

                    Method.ClearPunishment(dPlayer, action, id, server);
                    response = clearTranslation.PunishmentCleared.Replace("{player.name}", dPlayer.Name).Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}");
                    return true;
                default:
                    response = clearTranslation.ActionNotFounded;
                    return false;
            }
        }
    }
}