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

    public class WatchList : ICommand
    {
        public static WatchList Instance { get; } = new WatchList();

        public string Description { get; } = "Add or remove a player from the watchlist";

        public string Command { get; } = "watchlist";

        public string[] Aliases { get; } = { "wl" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            WatchListTranslation watchListTranslation = Plugin.Singleton.Config.Translation.WatchListTranslation;

            if (!sender.CheckPermission("ms.watchlist"))
            {
                response = watchListTranslation.InvalidPermission.Replace("{duration}", "ms.watchlist");
                return false;
            }

            if (arguments.Count == 0 || (arguments.Count == 1 && arguments.At(0) == "add"))
            {
                response = watchListTranslation.WrongUsage;
                return false;
            }

            switch (arguments.At(0))
            {
                case "add":
                    string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 1));
                    if (string.IsNullOrEmpty(reason))
                    {
                        response = watchListTranslation.ReasonNull;
                        return false;
                    }

                    HashSet<Collections.Player> targets = new();

                    if (arguments.At(0).Split(',').Length > 1)
                    {
                        foreach (string player in arguments.At(0).Split(','))
                        {
                            Collections.Player dPlayer = player.GetPlayer();
                            if (dPlayer is null)
                            {
                                response = watchListTranslation.PlayerNotFound.Replace("{target}", player);
                                continue;
                            }

                            if (targets.Contains(dPlayer)) continue;
                            targets.Add(dPlayer);
                        }
                    }
                    else
                    {
                        Collections.Player dPlayer = arguments.At(0).GetPlayer();
                        if (dPlayer == null)
                        {
                            response = watchListTranslation.PlayerNotFound.Replace("{player}", arguments.At(0));
                            return false;
                        }

                        if (!targets.Contains(dPlayer))
                            targets.Add(dPlayer);
                    }

                    foreach (Collections.Player player in targets)
                    {
                        ModerationSystemAPI.ApplyPunish(Player.Get(arguments.At(1)), ((CommandSender)sender).GetStaffer(), player,
                            PunishType.WatchList, reason, DateTime.MinValue.ToString(CultureInfo.InvariantCulture));
                        response = watchListTranslation.PlayerAddedWatchlist.Replace("{player.name}", player.Name)
                            .Replace("{player.userid}", $"{player.Id}@{player.Authentication}");
                    }
                    break;
                case "list":
                    if (arguments.Count == 1)
                    {
                        response = ModerationSystemAPI.GetWatchList(null);
                        return true;
                    }

                    Collections.Player target = arguments.At(1).GetPlayer();
                    if (target == null)
                    {
                        response = watchListTranslation.PlayerNotFound;
                        return false;
                    }

                    
                    response = ModerationSystemAPI.GetWatchList(target);
                    return true;

                default: response = watchListTranslation.ActionNotFounded;
                    return false;
            }

            response = "";
            return true;
        }
    }
}