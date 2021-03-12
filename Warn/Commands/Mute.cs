using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModerationSystem
{
    public class Mute : ICommand
    {
        private Mute()
        {
        }
        public static Mute Instance { get; } = new Mute();
        public string Command { get; } = "mute";
        public string[] Aliases { get; } = new[] { "m" };
        private CoroutineHandle coroutineHandle = new CoroutineHandle();
        public string Description { get; } = "Mute a player";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string reason;
            if (!sender.CheckPermission("ms.mute"))
            {
                response = "You can't do this command";
                return false;
            }

            if (arguments.Count < 3)
            {
                response = "Usage: ms mute/m <player name or id> <duration (minutes)> reason";
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
            var count = MuteCount(Database.LiteDatabase.GetCollection<MuteSystem>().Find(m => m.Target.Id == dPlayer.Id).ToList());
            var muteid = Convert.ToInt32(count);
            if (!int.TryParse(arguments.At(1), out int duration))
            {
                response = $"{duration} is not a valid duration!";
                return false;
            }
            reason = string.Join(" ", arguments.Array, 4, arguments.Array.Length - 4);
            new MuteSystem(dPlayer, issuer, reason, duration, DateTime.Now.ToString(), DateTime.Now.AddMinutes(duration).ToString(), muteid).Save();
            response = $"The player {target.Nickname} has been muted for: {duration} minute(s) with reason: {reason}";
            Log.Info($"Player {target.Nickname} ({target.UserId}) has been muted by {sender.LogName} for: {reason}");
            coroutineHandle = Timing.RunCoroutine(MutePlayer(duration * 60, target));
            target.Broadcast(Plugin.Singleton.Config.MuteMessage.Duration, Plugin.Singleton.Config.MuteMessage.Content.Replace("{duration}", duration.ToString()).Replace("{reason}", reason));
            return true;
        }

        private string MuteCount(List<MuteSystem> muteSystems)
        {
            muteSystems.Count().ToString();
            return muteSystems.Count().ToString();
        }

        private IEnumerator<float> MutePlayer(float duration, Exiled.API.Features.Player player)
        {
            player.IsMuted = true;
            player.IsIntercomMuted = true;
            yield return Timing.WaitForSeconds(duration);
            player.IsMuted = false;
            player.IsIntercomMuted = false;
        }

    }
}
