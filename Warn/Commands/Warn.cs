using CommandSystem;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModerationSystem
{
    public class Warn : ICommand
    {
        private Warn()
        {
        }
        public static Warn Instance { get; } = new Warn();
        public string Command { get; } = "warn";
        public string[] Aliases { get; } = new[] { "w" };
        public string Description { get; } = "warn a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string reason;
            if (!sender.CheckPermission("ms.warn"))
            {
                response = "You can't do this command!";
                return false;
            }
            if (arguments.Count < 2)
            {
                response = "Usage: ms warn/w <player name or id> reason";
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
            var warnid = WarnCount(Database.LiteDatabase.GetCollection<WarnSystem>().Find(w => w.Target.Id == dPlayer.Id).ToList());
            int i = Convert.ToInt32(warnid);

            reason = string.Join(" ", arguments.Array, 3, arguments.Array.Length - 3);
            new WarnSystem(dPlayer, issuer, reason, DateTime.Now.ToString(), i).Save();
            response = $"The player {target.Nickname} has been warned for: {reason}";
            target.Broadcast(Plugin.Singleton.Config.WarnMessage.Duration, Plugin.Singleton.Config.WarnMessage.Content.Replace("{reason}", reason));
            if (!Plugin.Singleton.Config.AutoKickEnable)
            {
                return false;
            }
            Kick(Database.LiteDatabase.GetCollection<WarnSystem>().Find(w => w.Target.Id == dPlayer.Id).ToList(), target, reason);
            return true;
        }

        private string WarnCount(List<WarnSystem> warnSystems)
        {
            warnSystems.Count().ToString();
            return warnSystems.Count().ToString();
        }

        private void Kick(List<WarnSystem> warnSystems, Exiled.API.Features.Player player, string reason)
        {            
                if (warnSystems.Count() == Plugin.Singleton.Config.MaximumWarn)
                {
                    player.Kick(Plugin.Singleton.Config.AutoKickMessage.Replace("{reason}", reason), "ModerationSystem");
                }
        }
    }
}
