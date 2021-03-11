using Exiled.API.Features;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;

namespace ModerationSystem
{
    public class Database
    {
        public static LiteDatabase LiteDatabase { get; private set; }
        public static string Folder => Path.Combine(Exiled.API.Features.Paths.Plugins, Plugin.Singleton.Config.DatabaseName);
        public static string FullDir => Path.Combine(Folder, $"{Plugin.Singleton.Config.DatabaseName}.db");

        public static Dictionary<Exiled.API.Features.Player, Player> PlayerData = new Dictionary<Exiled.API.Features.Player, Player>();
        private readonly Plugin plugin;
        public Database(Plugin plugin) => this.plugin = plugin;

        public void CreateDatabase()
        {
            if (Directory.Exists(Folder)) return;
            try
            {
                Directory.CreateDirectory(Folder);
                Log.Warn("I cannot found database, I'm creating one");
            }
            catch (Exception e)
            {
                Log.Error($"Error creating database: {e}");
            }
        }

        public void Open()
        {
            try
            {
                LiteDatabase = new LiteDatabase(FullDir);
                LiteDatabase.GetCollection<Player>().EnsureIndex(p => p.Id);
                LiteDatabase.GetCollection<Player>().EnsureIndex(p => p.Name);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to open the database: {e}");
            }
        }
    }
}
