using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ModerationSystem.Collections;
using static ModerationSystem.Database;

namespace ModerationSystem.Commands
{
    public class Unwarn : ICommand
    {
        private Unwarn()
        {
        }

        public static Unwarn Instance { get; } = new Unwarn();

        public string Description { get; } = "Remove a warn of player";

        public string Command { get; } = "unwarn";

        public string[] Aliases { get; } = { "uw" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.unwarn"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Usage: ms unwarn/uw <player name or ID> <ID>";
                return false;
            }

            var dPlayer = arguments.At(0).GetPlayer();
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

            var warnid = WarnCollection.FindOne(x => x.Target.Id == dPlayer.Id && x.Warnid == id);
            if (warnid == null)
            {
                
                response = "Warn ID not found";
                return false;
            }
            RemoveWarn(dPlayer, id);
            response = $"Warn {id} has been removed!";
            return true;

        }

        private void RemoveWarn(Player player, int id)
        {
            WarnCollection.DeleteMany(x => x.Warnid == id && x.Target == player);
        }
    }
}
