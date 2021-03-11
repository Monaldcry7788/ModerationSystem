using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModerationSystem
{
    public class Unmute : ICommand
    {
        private Unmute()
        {
        }
        public static Unmute Instance { get; } = new Unmute();
        public string Command { get; } = "unmute";
        public string[] Aliases { get; } = new[] { "um", "umute" };
        public string Description { get; } = "Mute a player";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.unmute"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage um/umute <player name or id> <mute id>";
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
            var muteid = Database.LiteDatabase.GetCollection<MuteSystem>().Find(x => x.Target.Id == dPlayer.Id && x.MuteId == id).ToList();
            if(!muteid.IsEmpty())
            {
                RemoveMute(dPlayer, id);
                response = "I have removed this mute!";
                target.IsMuted = false;
                target.IsIntercomMuted = false;
                return true;
            }
            response = "Mute ID not found";
            return false;
        }

        public void RemoveMute(Player player, int id)
        {
                Database.LiteDatabase.GetCollection<MuteSystem>().DeleteMany(x => x.MuteId == id);
        }
    }
}
