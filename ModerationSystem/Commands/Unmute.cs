using System;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;
using ModerationSystem.Collections;
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

        public string[] Aliases { get; } = { "um" };

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

            var dPlayer = arguments.At(0).GetPlayer();
            var issuer = ((CommandSender)sender).GetStaffer();
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

            var muteid = LiteDatabase.GetCollection<Collections.Mute>()
                .Find(x => x.Target.Id == dPlayer.Id && x.Muteid == id).ToList();
            if (!muteid.IsEmpty())
            {
                RemoveMute(dPlayer, id);
                response = $"Mute {id} has been removed!";
                return true;
            }

            response = "Mute ID not found";
            return false;
        }

        private void RemoveMute(Player player, int id)
        {
            LiteDatabase.GetCollection<Collections.Mute>().DeleteMany(x => x.Muteid == id);
        }
    }
}