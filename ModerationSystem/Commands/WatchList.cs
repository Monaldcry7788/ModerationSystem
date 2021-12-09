using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using ModerationSystem.Enums;

namespace ModerationSystem.Commands
{
    public class WatchList : ICommand
    {
        private WatchList()
        {
        }

        public static WatchList Instance { get; } = new WatchList();

        public string Description { get; } = "Add or remove a player from the watchlist";

        public string Command { get; } = "watchlist";

        public string[] Aliases { get; } = { "wl" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var watchListTranslation = Plugin.Singleton.Config.Translation.WatchListTranslation;
            if (!sender.CheckPermission("ms.watchlist"))
            {
                response = watchListTranslation.InvalidPermission.Replace("{duration}", "ms.watchlist");
                return false;
            }

            if (arguments.Count == 0 || arguments.Count > 3)
            {
                response = watchListTranslation.WrongUsage;
                return false;
            }

            var dPlayer = arguments.At(1).GetPlayer();
            if (dPlayer == null)
            {
                response = watchListTranslation.PlayerNotFound;
                return false;
            }
            var reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = watchListTranslation.ReasonNull;
                return false;
            }

            switch (arguments.At(0))
            {
                case "add":
                    Method.ApplyPunish(Player.Get(arguments.At(1)), ((CommandSender)sender).GetStaffer(), dPlayer,
                        PunishType.WatchList, reason, DateTime.MinValue);
                    response = watchListTranslation.PlayerAddedWatchlist.Replace("{player.name}", dPlayer.Name)
                        .Replace("{player.userid}", $"{dPlayer.Id}@{dPlayer.Authentication}");
                    return true;
                case "list":
                    if (arguments.Count == 1)
                    {
                        response = Method.GetWatchList(null);
                        return true;
                    }

                    response = Method.GetWatchList(dPlayer);
                    return true;
                default: response = watchListTranslation.ActionNotFounded;
                    return false;
            }
            response = "";
            return false;
        }
    }
}