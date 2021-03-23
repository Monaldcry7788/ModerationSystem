using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
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

        public string[] Aliases { get; } = new[] { "uw" };

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
            var warnid = LiteDatabase.GetCollection<Collections.Warn>().Find(x => x.Target.Id == dPlayer.Id && x.Warnid == id).ToList();
            if (!warnid.IsEmpty())
            {
                RemoveWarn(dPlayer, id);
                response = $"Warn {id} has been removed!";
                return true;
            }
            response = "Warn ID not found";
            return false;
        }

        private void RemoveWarn(Collections.Player player, int id)
        {
            LiteDatabase.GetCollection<Collections.Warn>().DeleteMany(x => x.Warnid == id);
        }
    }
}
