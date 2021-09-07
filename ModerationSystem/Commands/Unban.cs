using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ModerationSystem.Collections;
using static ModerationSystem.Database;

namespace ModerationSystem.Commands
{
    public class Unban : ICommand
    {
        private Unban()
        {
        }

        public static Unban Instance { get; } = new Unban();

        public string Description { get; } = "Unban a player";

        public string Command { get; } = "unban";

        public string[] Aliases { get; } = { "ub" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.unmute"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Usage: ms unban/ub <player name or ID> <ID>";
                return false;
            }

            var dPlayer = arguments.At(0).GetPlayer();
            var target = Exiled.API.Features.Player.Get(arguments.At(0));
            if (dPlayer == null)
            {
                response = "Player not found!";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out var id))
            {
                response = "Invalid ID";
                return false;
            }

            if (!dPlayer.IsBanned())
                response = $"Player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) isn't banned!";

            var banid = LiteDatabase.GetCollection<Collections.Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList();
            if (!banid.IsEmpty())
            {
                RemoveBan(dPlayer, id);
                response = $"Player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) unbanned!";
                return true;
            }

            response = "Ban ID not found!";
            return false;
        }

        private void RemoveBan(Player player, int id)
        {
            BanHandler.RemoveBan(player.Id+player.Authentication, BanHandler.BanType.UserId);
            LiteDatabase.GetCollection<Collections.Ban>().DeleteMany(x => x.Banid == id && player == x.Target);
        }
    }
}