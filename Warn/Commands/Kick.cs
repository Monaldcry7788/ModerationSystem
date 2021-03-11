using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModerationSystem
{
    public class Kick : ICommand
    {
        private Kick()
        {
        }
        public static Kick Instance { get; } = new Kick();
        public string Command { get; } = "kick";
        public string[] Aliases { get; } = new[] { "k" };
        public string Description { get; } = "Kick a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string reason;
            if (!sender.CheckPermission("ms.kick"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage kick/k <player name or id> reason";
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

            var kickid = KickCount(Database.LiteDatabase.GetCollection<KickSystem>().Find(k => k.Target.Id == dPlayer.Id).ToList());
            int i = Convert.ToInt32(kickid);
            reason = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
            new KickSystem(dPlayer, issuer, reason, DateTime.Now.ToString(), i).Save();
            response = $"The player {target.Nickname} has been kicked for: {reason}";
            target.Kick(Plugin.Singleton.Config.KickMessage.Replace("{reason}", reason));
            Log.Info($"Player {target.Nickname} ({target.UserId}) has been kicked by {sender.LogName} for reason: {reason}");
            return true;
        }

        private string KickCount(List<KickSystem> kickSystems)
        {
            kickSystems.Count().ToString();
            return kickSystems.Count().ToString();
        }
    }
}
