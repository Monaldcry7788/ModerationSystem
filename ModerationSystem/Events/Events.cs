using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace ModerationSystem.Events
{
    using static Database;

    internal class Events
    {

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
            if (!dPlayer.IsMuted() && MuteHandler.QueryPersistentMute(ev.Player.UserId))
            {
                MuteHandler.RevokePersistentMute(ev.Player.UserId);
                ev.Player.IsMuted = false;
                ev.Player.IsIntercomMuted = false;
                return;
            }
            if (dPlayer.IsMuted() && !ev.Player.IsMuted)
            {
                ev.Player.IsMuted = true;
                ev.Player.IsIntercomMuted = true;
            }
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            ev.Player.GetPlayer().Save();

            PlayerData.Remove(ev.Player);
        }
    }
}