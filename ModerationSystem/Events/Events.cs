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
    }
}