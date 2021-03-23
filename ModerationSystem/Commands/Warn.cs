namespace ModerationSystem.Commands
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Database;
    public class Warn : ICommand
    {
        private Warn()
        {
        }

        public static Warn Instance { get; } = new Warn();

        public string Description { get; } = "Warn a player";

        public string Command { get; } = "warn";

        public string[] Aliases { get; } = new[] { "w" };
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.warn"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: ms warn/w <player name or ID> <reason>";
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
            var i = WarnCount(LiteDatabase.GetCollection<Collections.Warn>().Find(w => w.Target.Id == dPlayer.Id).ToList());
            int warnid = Convert.ToInt32(i);
            new Collections.Warn(dPlayer, issuer, reason, DateTime.Now, warnid).Save();
            target?.Broadcast(Plugin.Singleton.Config.WarnMessage.Duration, Plugin.Singleton.Config.WarnMessage.Content.Replace("{reason}", reason));
            if (Plugin.Singleton.WebhookEnabled)
            {
                Webhook.Http.sendMessage(Plugin.Singleton.Config.WarnedMessageWebHook.Replace("{staffer}", sender.LogName).Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{reason}", reason).Replace("{warnid}", warnid.ToString()), "**New Warn**");
            }
            if (Plugin.Singleton.Config.AutoKickEnable)
            {
                Kick(LiteDatabase.GetCollection<Collections.Warn>().Find(x => x.Target.Id == dPlayer.Id).ToList(), target, reason);
            }
            response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been warned for: {reason}";
            return true;

        }

        private string WarnCount(List<Collections.Warn> warns)
        {
            warns.Count().ToString();
            return warns.Count().ToString();
        }

        private void Kick(List<Collections.Warn> warns, Exiled.API.Features.Player player, string reason)
        {
            if (warns.Count() == Plugin.Singleton.Config.MaximumWarn)
            {
                player.Disconnect(Plugin.Singleton.Config.AutoKickMessage.Replace("{reason}", reason));
            }
        }
    }
}
