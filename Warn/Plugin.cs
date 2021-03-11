using Exiled.API.Features;
using System;
using PlayerEvent = Exiled.Events.Handlers.Player;

namespace ModerationSystem
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "Twitch.tv/Monaldcry7788#9248";
        public override string Name { get; } = "ModerationSystem";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 8, 0);
        public static Plugin Singleton;
        public Database db;
        public Events Events;
        public Player Player { get; private set; }
        public Database PlayerDataDB { get; private set; }

        public override void OnEnabled()
        {
            Singleton = this;
            Events = new Events(this);
            db = new Database(this);
            PlayerDataDB = new Database(this);
            PlayerDataDB.CreateDatabase();
            PlayerDataDB.Open();
            PlayerEvent.Verified += Events.OnPlayerVerify;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;
            Events = null;
            db = null;
            PlayerDataDB = null;
            Database.LiteDatabase.Dispose();
            PlayerEvent.Verified -= Events.OnPlayerVerify;
            base.OnDisabled();
        }
    }
}
