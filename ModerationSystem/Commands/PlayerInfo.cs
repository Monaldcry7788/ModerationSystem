namespace ModerationSystem.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using ModerationSystem.Collections;
    using ModerationSystem.Configs.CommandTranslation;
    using NorthwoodLib.Pools;

    public class PlayerInfo : ICommand
    {
        public static PlayerInfo Instance { get; } = new PlayerInfo();

        public string Command { get; } = "playerinfo";

        public string[] Aliases { get; } = { "pi" };

        public string Description { get; } = "Show a list of infraction of a player";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            PlayerInfoTranslation playerInfoTranslation = Plugin.Singleton.Config.Translation.PlayerInfoTranslation;

            if (!sender.CheckPermission("ms.playerinfo"))
            {
                response = playerInfoTranslation.InvalidPermission.Replace("{permission}", "ms.playerinfo");
                return false;
            }

            if (arguments.Count != 1)
            {
                response = playerInfoTranslation.WrongUsage;
                return false;
            }

            Player dPlayer = arguments.At(0).GetPlayer();

            if (dPlayer == null)
            {
                response = playerInfoTranslation.PlayerNotFound;
                return false;
            }

            StringBuilder text = StringBuilderPool.Shared.Rent().AppendLine();

            text.AppendLine($"{dPlayer.Name} ({dPlayer.Id}@{dPlayer.Authentication})").AppendLine();

            string message = StringBuilderPool.Shared.ToStringReturn(text) + GetSanctions(
                Database.Database.WarnCollection.Find(w => w.Target.Id == dPlayer.Id),
                Database.Database.MuteCollection.Find(w => w.Target.Id == dPlayer.Id),
                Database.Database.KickCollection.Find(w => w.Target.Id == dPlayer.Id),
                Database.Database.BanCollection.Find(w => w.Target.Id == dPlayer.Id),
                Database.Database.SoftBanCollection.Find(w => w.Target.Id == dPlayer.Id),
                Database.Database.SoftWarnCollection.Find(w => w.Target.Id == dPlayer.Id));
                          response = message;
            return true;
        }

        private string GetSanctions(IEnumerable<Collections.Warn> warns, IEnumerable<Collections.Mute> mutes, IEnumerable<Collections.Kick> kicks, IEnumerable<Collections.Ban> bans, IEnumerable<Collections.SoftBan> softBans, IEnumerable<Collections.SoftWarn> softWarns)
        {
            StringBuilder message = StringBuilderPool.Shared.Rent();

            message.Append($"[Warns: ({warns.Count()})]").AppendLine().AppendLine();
            foreach (Collections.Warn p in warns)
                message.AppendLine($"Warn ID: {p.WarnId}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Date: {p.Date}").AppendLine($"Server: {p.Server}").AppendLine();

            message.Append($"[Mutes: ({mutes.Count()})]").AppendLine().AppendLine();
            foreach (Collections.Mute p in mutes)
                message.AppendLine($"Mute ID: {p.MuteId}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Duration: {p.Duration}").AppendLine($"Date: {p.Date}").AppendLine($"Expire: {p.Expire}").AppendLine($"Server: {p.Server}").AppendLine();

            message.Append($"[Kick: ({kicks.Count()})]").AppendLine().AppendLine();
            foreach (Collections.Kick p in kicks)
                message.AppendLine($"Kick ID: {p.KickId}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Date: {p.Date}").AppendLine($"Server: {p.Server}").AppendLine();

            message.Append($"[Ban: ({bans.Count()})]").AppendLine().AppendLine();
            foreach (Collections.Ban p in bans)
                message.AppendLine($"Warn ID: {p.BanId}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Duration: {p.Duration}").AppendLine($"Date: {p.Date}").AppendLine($"Expire: {p.Expire}").AppendLine($"Server: {p.Server}").AppendLine();

            message.Append($"[SoftBan: ({softBans.Count()})]").AppendLine().AppendLine();
            foreach (Collections.SoftBan p in softBans)
                message.AppendLine($"SoftBan ID: {p.SoftBanId}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Duration: {p.Duration}").AppendLine($"Date: {p.Date}").AppendLine($"Expire: {p.Expire}").AppendLine($"Server: {p.Server}").AppendLine();

            message.Append($"[SoftWarns: ({softWarns.Count()})]").AppendLine().AppendLine();
            foreach (Collections.SoftWarn p in softWarns)
                message.AppendLine($"SoftWarn ID: {p.SoftWarnId}").AppendLine($"Reason: {p.Reason}").AppendLine($"Issuer: {p.Issuer.Name}").AppendLine($"Date: {p.Date}").AppendLine($"Server: {p.Server}").AppendLine();
 
            return StringBuilderPool.Shared.ToStringReturn(message);
        }
    }
}