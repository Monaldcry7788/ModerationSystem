using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Linq;
using static ModerationSystem.Database;

namespace ModerationSystem.Commands
{
    public class Unmute : ICommand
    {
        private Unmute()
        {
        }

        public static Unmute Instance { get; } = new Unmute();

        public string Description { get; } = "Unmute a player";

        public string Command { get; } = "unmute";

        public string[] Aliases { get; } = new[] { "um" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.unmute"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count != 2)
            {
                response = "Usage: ms unmute/um <player name or ID> <ID>";
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
            var muteid = LiteDatabase.GetCollection<Collections.Mute>().Find(x => x.Target.Id == dPlayer.Id && x.Muteid == id).ToList();
            if (!muteid.IsEmpty())
            {
                RemoveMute(dPlayer, id);
                response = $"Mute {id} has been removed!";
                return true;
            }
            response = "Mute ID not found";
            return false;
        }

        private void RemoveMute(Collections.Player player, int id)
        {
            LiteDatabase.GetCollection<Collections.Mute>().DeleteMany(x => x.Muteid == id);
        }
    }
}
