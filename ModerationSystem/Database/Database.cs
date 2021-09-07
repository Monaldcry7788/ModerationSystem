using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using LiteDB;
using ModerationSystem.Collections;
using Player = ModerationSystem.Collections.Player;

namespace ModerationSystem
{
    internal static class Database
    {
        public static Player ServerPlayer = new Player("Server", "Server", "Server");
        public static LiteDatabase LiteDatabase { get; private set; }

        public static Dictionary<Exiled.API.Features.Player, Player> PlayerData { get; } =
            new Dictionary<Exiled.API.Features.Player, Player>();

        public static string Folder => Path.Combine(Paths.Plugins, Plugin.Singleton.Config.DatabaseName);
        public static string FullPath => Path.Combine(Folder, $"{Plugin.Singleton.Config.DatabaseName}.db");

        public static void Open()
        {
            try
            {
                if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);

                LiteDatabase = new LiteDatabase(FullPath);

                LiteDatabase.GetCollection<Player>().EnsureIndex(player => player.Id, true);
                LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Target.Id);
                LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Issuer.Id);
                LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Date);
                LiteDatabase.GetCollection<Mute>().EnsureIndex(mute => mute.Expire);
                LiteDatabase.GetCollection<Warn>().EnsureIndex(warn => warn.Target.Id);
                LiteDatabase.GetCollection<Warn>().EnsureIndex(warn => warn.Issuer.Id);
                LiteDatabase.GetCollection<Warn>().EnsureIndex(warn => warn.Date);
                LiteDatabase.GetCollection<Kick>().EnsureIndex(warn => warn.Target.Id);
                LiteDatabase.GetCollection<Kick>().EnsureIndex(warn => warn.Issuer.Id);
                LiteDatabase.GetCollection<Kick>().EnsureIndex(warn => warn.Date);
                LiteDatabase.GetCollection<Ban>().EnsureIndex(warn => warn.Target.Id);
                LiteDatabase.GetCollection<Ban>().EnsureIndex(warn => warn.Issuer.Id);
                LiteDatabase.GetCollection<Ban>().EnsureIndex(warn => warn.Date);
                LiteDatabase.GetCollection<Ban>().EnsureIndex(warn => warn.Expire);


                Log.Info("Database Loaded!");
            }
            catch (Exception e)
            {
                Log.Error($"Error when try to open database:\n {e}");
            }
        }

        public static void Close()
        {
            try
            {
                LiteDatabase.Checkpoint();
                LiteDatabase.Dispose();
                LiteDatabase = null;

                Log.Info("Database closed!");
            }
            catch (Exception e)
            {
                Log.Error($"Error when try to close database:\n {e}");
            }
        }
    }
}