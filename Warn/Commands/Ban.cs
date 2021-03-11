using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModerationSystem
{
    public class Ban : ICommand
    {
        private Ban()
        {
        }
        public static Ban Instance { get; } = new Ban();
        public string Command { get; } = "ban";
        public string[] Aliases { get; } = new[] { "b" };
        public string Description { get; } = "Ban a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string reason;
            if (!sender.CheckPermission("ms.ban"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 3)
            {
                response = "Usage ban/b <player name or id> <duration (minutes)> reason";
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


            var count = BanCount(Database.LiteDatabase.GetCollection<BanSystem>().Find(k => k.Target.Id == dPlayer.Id).ToList());
            int banid = Convert.ToInt32(count);
            if (!int.TryParse(arguments.At(1), out int duration))
            {
                response = $"{duration} is not a valid duration!";
                return false;
            }
            reason = string.Join(" ", arguments.Array, 4, arguments.Array.Length - 4);
            new BanSystem(dPlayer, issuer, reason, duration, DateTime.Now.ToString(), DateTime.Now.AddMinutes(duration).ToString(), banid).Save();
            response = $"The player {target.Nickname} has been banned for {duration} minute(s) with reason: {reason}";
            target.Ban(duration * 60, Plugin.Singleton.Config.BanMessage.Replace("{reason}", reason));
            Log.Info($"Player {target.Nickname} ({target.UserId}) has been banned by {sender.LogName} for {duration} minute(s) with reason: {reason}");
            return true;
        }

        private string BanCount(List<BanSystem> banSystems)
        {
            banSystems.Count().ToString();
            return banSystems.Count().ToString();
        }
    }
}
