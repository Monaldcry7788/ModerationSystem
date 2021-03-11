using CommandSystem;
using Exiled.Permissions.Extensions;
using NorthwoodLib.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModerationSystem
{
    public class PlayerInfo : ICommand
    {
        public static PlayerInfo Instance { get; } = new PlayerInfo();
        public string Command { get; } = "playerinfo";
        public string[] Aliases { get; } = new[] { "ip", "pi", "pinfo" };
        public string Description { get; } = "show a list of infractions";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {

            if (!sender.CheckPermission("ms.playerinfo"))
            {
                response = "You can't do this command!";
                return false;
            }
            if (arguments.Count != 1)
            {
                response = "Usage: warns/ws <player name or id>";
                return false;
            }

            Player dPlayer = arguments.At(0).GetPlayer();
            if (dPlayer == null)
            {
                response = "Player not found";
                return false;
            }
            StringBuilder text = StringBuilderPool.Shared.Rent().AppendLine();
            text.AppendLine($"{dPlayer.Name} ({dPlayer.Id}@{dPlayer.Auth})").AppendLine();
            var Response = StringBuilderPool.Shared.ToStringReturn(text) + GetPlayerWarned(Database.LiteDatabase.GetCollection<WarnSystem>().Find(w => w.Target.Id == dPlayer.Id).ToList()) + GetPlayerKicked(Database.LiteDatabase.GetCollection<KickSystem>().Find(k => k.Target.Id == dPlayer.Id).ToList()) + GetPlayerBanned(Database.LiteDatabase.GetCollection<BanSystem>().Find(b => b.Target.Id == dPlayer.Id).ToList()) + GetPlayerMuted(Database.LiteDatabase.GetCollection<MuteSystem>().Find(m => m.Target.Id == dPlayer.Id).ToList());
            response = Response;
            return true;
        }

        private string GetPlayerWarned(List<WarnSystem> warnSystems)
        {
            StringBuilder message = StringBuilderPool.Shared.Rent();
            message.AppendLine($"[Warns: ({warnSystems.Count()})]").AppendLine();
            foreach (var pWarn in warnSystems)
            {
                message.AppendLine($"Warn ID: {pWarn.WarnId}")
                .AppendLine($"Reason: {pWarn.Reason}")
                .AppendLine($"Issuer: {pWarn.Issuer.Name}")
                .AppendLine($"Date: {pWarn.Date}").AppendLine();
            }
            return StringBuilderPool.Shared.ToStringReturn(message);
        }

        private string GetPlayerKicked(List<KickSystem> kickSystems)
        {
            StringBuilder message = StringBuilderPool.Shared.Rent();
            message.AppendLine($"[Kick: ({kickSystems.Count()})]").AppendLine();
            foreach (var pKick in kickSystems)
            {
                message.AppendLine($"Kick ID: {pKick.KickId}")
                    .AppendLine($"Reason: {pKick.Reason}")
                    .AppendLine($"Issuer: {pKick.Issuer.Name}")
                    .AppendLine($"Date: {pKick.Date}").AppendLine();
            }
            return StringBuilderPool.Shared.ToStringReturn(message);
        }
        private string GetPlayerBanned(List<BanSystem> banSystems)
        {
            StringBuilder message = StringBuilderPool.Shared.Rent();
            message.AppendLine($"[Ban: ({banSystems.Count()})]").AppendLine();
            foreach (var pBan in banSystems)
            {
                message.AppendLine($"Ban ID: {pBan.BanId}")
                    .AppendLine($"Reason: {pBan.Reason}")
                    .AppendLine($"Issuer: {pBan.Issuer.Name}")
                    .AppendLine($"Duration: {pBan.Duration} minute(s)")
                    .AppendLine($"Date: {pBan.Date}")
                    .AppendLine($"Expire: {pBan.Expire}").AppendLine();
            }
            return StringBuilderPool.Shared.ToStringReturn(message);
        }

        private string GetPlayerMuted(List<MuteSystem> muteSystems)
        {
            StringBuilder message = StringBuilderPool.Shared.Rent();
            message.AppendLine($"[Mute: ({muteSystems.Count()})]").AppendLine();
            foreach (var pMute in muteSystems)
            {
                message.AppendLine($"Mute ID: {pMute.MuteId}")
                    .AppendLine($"Reason: {pMute.Reason}")
                    .AppendLine($"Issuer: {pMute.Issuer.Name}")
                    .AppendLine($"Duration: {pMute.Duration} minute(s)")
                    .AppendLine($"Date: {pMute.Date}")
                    .AppendLine($"Expire: {pMute.Expire}").AppendLine();
            }
            return StringBuilderPool.Shared.ToStringReturn(message);
        }
    }
}

