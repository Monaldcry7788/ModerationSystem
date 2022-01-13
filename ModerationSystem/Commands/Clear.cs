namespace ModerationSystem.Commands
{
    using System;
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using ModerationSystem.Configs.CommandTranslation;
    using ModerationSystem.Collections;
    using ModerationSystem.Enums;

    public class Clear : ICommand
    {
        public static Clear Instance { get; } = new Clear();

        public string Description { get; } = "Clear a player";

        public string Command { get; } = "clear";

        public string[] Aliases { get; } = { "c" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            ClearTranslation clearTranslation = Plugin.Singleton.Config.Translation.ClearTranslation;
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

            Player dPlayer = arguments.At(0).GetPlayer();
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
                    PunishType? action = arguments.At(1).GetPunishType();
                    if (!int.TryParse(arguments.At(2), out int id))
                    {
                        response = clearTranslation.IdNotFound;
                        return false;
                    }

                    if (!int.TryParse(arguments.At(3), out int server))
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