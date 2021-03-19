namespace ModerationSystem.Events
{
    using Exiled.Events.EventArgs;
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
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {

            ev.Player.GetPlayer().Save();

            PlayerData.Remove(ev.Player);
        }
    }
}
