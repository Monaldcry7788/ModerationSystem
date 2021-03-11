using Exiled.Events.EventArgs;

namespace ModerationSystem
{
    public class Events
    {
        private readonly Plugin plugin;
        public Events(Plugin plugin) => this.plugin = plugin;

        public void OnPlayerVerify(VerifiedEventArgs ev)
        {
            var dPlayer = ev.Player.GetPlayer() ?? new Player(
                ev.Player.RawUserId,
                ev.Player.AuthenticationType.ToString().ToLower(),
                ev.Player.Nickname
                );
            Database.PlayerData.Add(ev.Player, dPlayer);
            dPlayer.Name = ev.Player.Nickname;
            dPlayer.Save();
        }
    }
}
