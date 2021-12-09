using System;
using System.IO;
using Exiled.API.Features;
using Player = Exiled.Events.Handlers.Player;

namespace ModerationSystem
{
    public class Plugin : Plugin<Configs.Config>
    {
        public static Plugin Singleton;

        internal Events.Events Events { get; private set; }
        public override string Author => "Monald#9248";
        public override string Name => "ModerationSystem";
        public override Version Version => new Version(3, 0, 0);
        public override Version RequiredExiledVersion => new Version(4, 0, 0);
        private FileSystemWatcher _fileSystemWatcher;

        public override void OnEnabled()
        {
            Singleton = this;
            Events = new Events.Events();
            Player.Verified += ModerationSystem.Events.Events.OnVerified;
            Player.Destroying += ModerationSystem.Events.Events.OnDestroying;
            Player.ChangingRole += ModerationSystem.Events.Events.OnChangingRole;

            Database.Open();

            if (Config.IsDatabaseGlobal)
            {
                if (!Directory.Exists(Database.CacheFolder))
                    Directory.CreateDirectory(Database.CacheFolder);
                _fileSystemWatcher = new FileSystemWatcher(Database.CacheFolder)
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    Filter = "*.json",
                    EnableRaisingEvents = true
                };
                _fileSystemWatcher.Created += ModerationSystem.Events.Events.OnFileChanged;

            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Player.Verified -= ModerationSystem.Events.Events.OnVerified;
            Player.Destroying -= ModerationSystem.Events.Events.OnDestroying;
            Player.ChangingRole -= ModerationSystem.Events.Events.OnChangingRole;
            Events = null;
            if (Config.IsDatabaseGlobal)
            {
                _fileSystemWatcher.Changed -= ModerationSystem.Events.Events.OnFileChanged;
                _fileSystemWatcher = null;
            }
            
            Database.Close();
            
            Singleton = null;

            base.OnDisabled();
        }
    }
}
