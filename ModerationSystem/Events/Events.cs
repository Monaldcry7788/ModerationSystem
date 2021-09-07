using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace ModerationSystem.Events
{
    using static Database;

    internal class Events
    {
        public Dictionary<Player, int> teamkill = new Dictionary<Player, int>();

        public void OnVerified(VerifiedEventArgs ev)
        {
            var dPlayer = ev.Player.GetPlayer() ?? new Collections.Player(
                ev.Player.RawUserId,
                ev.Player.AuthenticationType.ToString().ToLower(),
                ev.Player.Nickname
            );
            PlayerData.Add(ev.Player, dPlayer);

            if (dPlayer.Name != ev.Player.Nickname)
            {
                dPlayer.Name = ev.Player.Nickname;
                dPlayer.Save();
            }
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            ev.Player.GetPlayer().Save();

            PlayerData.Remove(ev.Player);
        }

        public void OnDying(DyingEventArgs e)
        {
            if (!Plugin.Singleton.Config.IsAntiTeamKillEnabled) return;
            if (e.Killer.Team != e.Target.Team) return;
            if (e.Killer == e.Target) return;
            if (RoundSummary.singleton.RoundEnded) return;
            if (e.HitInformation.Tool.Name == "WALL") return;
            if (!teamkill.ContainsKey(e.Killer)) teamkill.Add(e.Killer, 0);
            var playerinfo = teamkill[e.Killer];
            Log.Debug(playerinfo);
            if (playerinfo > Plugin.Singleton.Config.ReverseTeamKillNumber)
            {
                switch (Plugin.Singleton.Config.Action)
                {
                    case "warn":
                        Method.Warn(e.Killer, ServerPlayer, e.Killer.GetPlayer(),
                            Plugin.Singleton.Config.ActionReason["warn"]);
                        break;
                    case "kick":
                        Method.Kick(e.Killer, ServerPlayer, e.Killer.GetPlayer(),
                            Plugin.Singleton.Config.ActionReason["kick"]);
                        break;
                    case "ban":
                        Method.Ban(e.Killer, ServerPlayer, e.Killer.GetPlayer(), Plugin.Singleton.Config.ActionReason["ban"], Plugin.Singleton.Config.BanDuration * 60);
                        break;
                    case "nothing":
                        break;
                }

                e.IsAllowed = false;
                return;
            }
            playerinfo = playerinfo+1;
            teamkill[e.Killer] = playerinfo;
        }
    }
}