using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
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

        public string[] Aliases { get; } = new[] { "ub" };

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
            Collections.Player dPlayer = arguments.At(0).GetPlayer();
            Collections.Player issuer = ((CommandSender)sender).GetStaffer();
            Player target = Player.Get(arguments.At(0));
            if (dPlayer == null)
            {
                response = "Player not found!";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int id))
            {
                response = "Invalid ID";
                return false;

            }
            if (!dPlayer.IsBanned())
            {
                response = $"Player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) isn't banned!";
            }

            var banid = LiteDatabase.GetCollection<Collections.Ban>().Find(x => x.Target.Id == dPlayer.Id && x.Banid == id).ToList();
            if (!banid.IsEmpty())
            {
                RemoveBan(dPlayer, target, id);
                response = $"Player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) unbanned!";
                return true;
            }
            response = "Ban ID not found!";
            return false;
        }

        private void RemoveBan(Collections.Player chatPlayer, Player player, int id)
        {
            BanHandler.RemoveBan(player.IPAddress, BanHandler.BanType.IP);
            BanHandler.RemoveBan(player.UserId, BanHandler.BanType.UserId);
            LiteDatabase.GetCollection<Collections.Ban>().DeleteMany(x => x.Banid == id);

        }
    }
}
