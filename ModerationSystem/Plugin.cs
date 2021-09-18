using System;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;

namespace ModerationSystem
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Singleton;

        internal Events.Events Events { get; private set; }
        public override string Author { get; } = "Monaldcry7788#9248";
        public override string Name { get; } = "ModerationSystem";
        public override Version Version { get; } = new Version(2, 0, 2);
        public override Version RequiredExiledVersion { get; } = new Version(2, 14, 0);

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
