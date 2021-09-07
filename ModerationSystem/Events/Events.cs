using System.Collections.Generic;
using Exiled.API.Features;

namespace ModerationSystem.Events
{
    using Exiled.Events.EventArgs;
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
            if (!teamkill.ContainsKey(e.Killer)) teamkill.Add(e.Killer, 0);
            int playerinfo = teamkill[e.Killer];
            if (playerinfo < Plugin.Singleton.Config.ReverseTeamKillNumber)
                {
                    e.IsAllowed = false;
                    playerinfo++;
                    teamkill[e.Killer] = playerinfo;
                    return;
                }

            switch (Plugin.Singleton.Config.Action)
            {
                case "warn":
                    
            }
        }
    }
}
