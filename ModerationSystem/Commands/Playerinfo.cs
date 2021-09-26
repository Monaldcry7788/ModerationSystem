using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;
namespace ModerationSystem.Commands
{
    public class Playerinfo : ICommand
    {
        private Playerinfo()
        {
        }

        public static Playerinfo Instance { get; } = new Playerinfo();

        public string Command { get; } = "playerinfo";

        public string[] Aliases { get; } = { "pi" };

        public string Description { get; } = "Show a list of infraction of a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("ms.playerinfo"))
            {
                response = "You can't do this command!";
                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: ms playerinfo/pi <player name or ID>";
                return false;
            }
            var dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = "Player not found!!";
                return false;
            }

            var text = StringBuilderPool.Shared.Rent().AppendLine();
            text.AppendLine($"{dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication})").AppendLine();
            var message = StringBuilderPool.Shared.ToStringReturn(text) +
                          GetWarn(Database.WarnCollection
                              .Find(w => w.Target.Id == dPlayer.Id).ToList()) +
                          GetKick(Database.KickCollection
                              .Find(k => k.Target.Id == dPlayer.Id).ToList()) +
                          GetBan(Database.BanCollection
                              .Find(b => b.Target.Id == dPlayer.Id).ToList()) + GetMute(Database.MuteCollection.Find(m => m.Target.Id == dPlayer.Id).ToList());
            response = message;
            return true;
        }

        private string GetWarn(List<Collections.Warn> warns)
        {
            var message = StringBuilderPool.Shared.Rent();
            message.Append($"[Warns: ({warns.Count})]").AppendLine().AppendLine();
            foreach (var p in warns)
                message.AppendLine($"Warn ID: {p.Warnid}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Date: {p.Date}").AppendLine();
            return StringBuilderPool.Shared.ToStringReturn(message);
        }

        private string GetMute(List<Collections.Mute> mutes)
        {
            var message = StringBuilderPool.Shared.Rent();
            message.Append($"[Mutes: ({mutes.Count})]").AppendLine().AppendLine();
            foreach (var p in mutes)
                message.AppendLine($"Mute ID: {p.Muteid}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Duration: {p.Duration}").AppendLine($"Date: {p.Date}").AppendLine($"Expire: {p.Expire}").AppendLine();
            return StringBuilderPool.Shared.ToStringReturn(message);
        }

        private string GetKick(List<Collections.Kick> kicks)
        {
            var message = StringBuilderPool.Shared.Rent();
            message.Append($"[Kick: ({kicks.Count})]").AppendLine().AppendLine();
            foreach (var p in kicks)
                message.AppendLine($"Kick ID: {p.Kickid}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Date: {p.Date}").AppendLine();
            return StringBuilderPool.Shared.ToStringReturn(message);
        }

        private string GetBan(List<Collections.Ban> bans)
        {
            var message = StringBuilderPool.Shared.Rent();
            message.Append($"[Ban: ({bans.Count})]").AppendLine().AppendLine();
            foreach (var p in bans)
                message.AppendLine($"Warn ID: {p.Banid}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Duration: {p.Duration}").AppendLine($"Date: {p.Date}").AppendLine($"Expire: {p.Expire}").AppendLine();
            return StringBuilderPool.Shared.ToStringReturn(message);
        }
    }
}