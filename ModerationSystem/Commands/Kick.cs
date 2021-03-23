using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static ModerationSystem.Database;

namespace ModerationSystem.Commands
{
    public class Kick : ICommand
    {
        private Kick()
        {
        }

        public static Kick Instance { get; } = new Kick();

        public string Description { get; } = "Kick a player";

        public string Command { get; } = "kick";

        public string[] Aliases { get; } = new[] { "k" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.kick"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: ms kick/k <player name or ID> <reason>";
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
            string reason = string.Join(" ", arguments.Skip(1).Take(arguments.Count - 1));
            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
                return false;
            }

            List<Player> players = Player.List.Where(x => x.RawUserId == dPlayer.Id).ToList();
            if (!players.IsEmpty())
            {
                var i = KickCount(LiteDatabase.GetCollection<Collections.Kick>().Find(x => x.Target.Id == dPlayer.Id).ToList());
                var kickid = Convert.ToInt32(i);
                new Collections.Kick(dPlayer, issuer, reason, DateTime.Now, kickid).Save();
                target?.Disconnect(Plugin.Singleton.Config.KickMessage.Replace("{reason}", reason));
                if (Plugin.Singleton.WebhookEnabled)
                {
                    Webhook.Http.sendMessage(Plugin.Singleton.Config.KickedMessageWebHook.Replace("{staffer}", sender.LogName).Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{reason}", reason).Replace("{kickid}", kickid.ToString()), "New Kick!");
                }
                response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been kicked for: {reason}";
                return true;
            }
            response = "Player not found!";
            return false;
        }

        private string KickCount(List<Collections.Kick> kicks)
        {
            kicks.Count().ToString();
            return kicks.Count().ToString();
        }
    }
}
