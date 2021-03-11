using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModerationSystem
{
    public class WarnRemove : ICommand
    {
        private WarnRemove()
        {
        }

        public static WarnRemove Instance { get; } = new WarnRemove();
        public string Command { get; } = "unwarn";
        public string[] Aliases { get; } = new[] { "uw", "uwarn" };
        public string Description { get; } = "Unwarn a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.unwarn"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: uw/unwarn <player name or id> <mute id>";
                return false;
            }

            Exiled.API.Features.Player target = Exiled.API.Features.Player.Get(arguments.At(0));
            Player dPlayer = arguments.At(0).GetPlayer();
            Player issuer = ((CommandSender)sender).GetStaff();
            if (dPlayer == null)
            {
                response = "Player not found";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int id))
            {
                response = $"{id} is not a valid duration!";
                return false;
            }

            var warnid = Database.LiteDatabase.GetCollection<WarnSystem>().Find(x => x.Target.Id == dPlayer.Id && x.WarnId == id).ToList();
            if (!warnid.IsEmpty())
            {
                RemoveWarn(dPlayer, id);
                response = "I have removed this warn";
                return true;
            }
            response = "Warn ID not found";
            return false;

        }

        public void RemoveWarn(Player player, int id)
        {
            Database.LiteDatabase.GetCollection<WarnSystem>().DeleteMany(x => x.WarnId == id);
        }
    }
}
