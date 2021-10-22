using System;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;

namespace ModerationSystem
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Singleton;

        internal Events.Events Events { get; private set; }
        public override string Author => "Monald#9248";
        public override string Name => "ModerationSystem";
        public override Version Version => new Version(2, 0, 3);
        public override Version RequiredExiledVersion => new Version(3, 0, 0);

        public override void OnEnabled()
        {
            Singleton = this;

            Events = new Events.Events();
            Player.Verified += Events.OnVerified;
            Player.Destroying += Events.OnDestroying;

            Database.Open();
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= Events.OnVerified;
            Player.Destroying -= Events.OnDestroying;
            Events = null;

            Database.Close();
            
            Singleton = null;

            base.OnDisabled();
        }
    }
}
