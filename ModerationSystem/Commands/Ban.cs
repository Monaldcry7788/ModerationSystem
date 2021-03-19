using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static ModerationSystem.Database;

namespace ModerationSystem.Commands
{
    public class Ban : ICommand
    {
        private Ban()
        {
        }

        public static Ban Instance { get; } = new Ban();

        public string Description { get; } = "Ban a player";

        public string Command { get; } = "ban";

        public string[] Aliases { get; } = new[] { "b" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.ban"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: ms ban/b <player name or ID> <duration (minutes)> <reason>";
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
            string reason = string.Join(" ", arguments.Skip(2).Take(arguments.Count - 2));
            if (string.IsNullOrEmpty(reason))
            {
                response = "Reason can't be null";
                return false;
            }

            if (!int.TryParse(arguments.At(1), out int duration))
            {
                response = $"{duration} isn't a valid duration!";
                return false;
            }

            if (dPlayer.IsBanned())
            {
                response = "Player is already banned!";
                return false;
            }

            var i = BanCount(LiteDatabase.GetCollection<Collections.Ban>().Find(x => x.Target.Id == dPlayer.Id).ToList());
            var banid = Convert.ToInt32(i);
            new Collections.Ban(dPlayer, issuer, reason, duration, DateTime.Now, DateTime.Now.AddMinutes(duration), banid).Save();
            BanHandler.IssueBan(new BanDetails()
            {
                Expires = DateTime.UtcNow.AddMinutes(duration).Ticks,
                Id = dPlayer.Id + "@" + dPlayer.Authentication,
                IssuanceTime = DateTime.Now.Ticks,
                Reason = reason,
                Issuer = issuer.Name,
                OriginalName = dPlayer.Name
            }, BanHandler.BanType.UserId);
            target?.Disconnect($"You has been banned!: {reason}");
            if (Plugin.Singleton.WebhookEnabled)
            {
                Webhook.Http.sendMessage(Plugin.Singleton.Config.BanMessageWebHook.Replace("{staffer}", sender.LogName).Replace("{target.Name}", dPlayer.Name).Replace("{target.Id}", dPlayer.Id + "@" + dPlayer.Authentication).Replace("{duration}", duration.ToString()).Replace("{reason}", reason).Replace("{banid}", banid.ToString()), "New Ban!");
            }
            response = $"The player {dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication}) has been banned for {duration} minute(s) with reason: {reason}";
            return true;
        }

        private string BanCount(List<Collections.Ban> bans)
        {
            bans.Count().ToString();
            return bans.Count().ToString();
        }
    }
}
