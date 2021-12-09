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
        public static Player ServerPlayer = new Player(null, null, "Server");
        public static LiteDatabase LiteDatabase { get; private set; }
        public static ILiteCollection<Player> PlayerCollection { get; private set; }
        public static ILiteCollection<Warn> WarnCollection { get; private set; }
        public static ILiteCollection<Kick> KickCollection { get; private set; }
        public static ILiteCollection<Mute> MuteCollection { get; private set; }
        public static ILiteCollection<Ban> BanCollection { get; private set; }
        public static ILiteCollection<SoftBan> SoftBanCollection { get; private set; }
        public static ILiteCollection<SoftWarn> SoftWarnCollection { get; private set; }
        public static ILiteCollection<WatchList> WatchListCollection { get; private set; }

        public static Dictionary<Exiled.API.Features.Player, Player> PlayerData { get; } = new Dictionary<Exiled.API.Features.Player, Player>();

        public static string GlobalFolder => Path.Combine(Paths.Plugins, "ModerationSystem");
        public static string Folder => Path.Combine(Path.Combine(GlobalFolder, $"ModerationSystem-{Server.Port}"));
        public static string DatabasePath => Path.Combine(Folder, $"{Plugin.Singleton.Config.DatabaseName}-{Server.Port}.db");
        public static string CacheFolder => Path.Combine(GlobalFolder, "Cache");

        public static void Open()
        {
            try
            {
                if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);

                LiteDatabase = new LiteDatabase(DatabasePath);
                PlayerCollection = LiteDatabase.GetCollection<Player>();
                WarnCollection = LiteDatabase.GetCollection<Warn>();
                KickCollection = LiteDatabase.GetCollection<Kick>();
                MuteCollection = LiteDatabase.GetCollection<Mute>();
                BanCollection = LiteDatabase.GetCollection<Ban>();
                SoftBanCollection = LiteDatabase.GetCollection<SoftBan>();
                SoftWarnCollection = LiteDatabase.GetCollection<SoftWarn>();
                WatchListCollection = LiteDatabase.GetCollection<WatchList>();
                
                PlayerCollection.EnsureIndex(p => p.Id, true);
                WarnCollection.EnsureIndex(w => w.Target.Id);
                WarnCollection.EnsureIndex(w => w.Issuer.Id);
                WarnCollection.EnsureIndex(w => w.Date);
                KickCollection.EnsureIndex(k => k.Target.Id);
                KickCollection.EnsureIndex(k => k.Issuer.Id);
                KickCollection.EnsureIndex(k => k.Date);
                MuteCollection.EnsureIndex(m => m.Target.Id);
                MuteCollection.EnsureIndex(m => m.Issuer.Id);
                MuteCollection.EnsureIndex(m => m.Date);
                MuteCollection.EnsureIndex(m => m.Expire);
                BanCollection.EnsureIndex(b => b.Target.Id);
                BanCollection.EnsureIndex(b => b.Issuer.Id);
                BanCollection.EnsureIndex(b => b.Date);
                BanCollection.EnsureIndex(b => b.Expire);
                SoftBanCollection.EnsureIndex(sb => sb.Target.Id);
                SoftBanCollection.EnsureIndex(sb => sb.Issuer.Id);
                SoftBanCollection.EnsureIndex(sb => sb.Date);
                SoftBanCollection.EnsureIndex(sb => sb.Expire);
                SoftWarnCollection.EnsureIndex(sw => sw.Target.Id);
                SoftWarnCollection.EnsureIndex(sw => sw.Issuer.Id);
                SoftWarnCollection.EnsureIndex(sw => sw.Date);
                WatchListCollection.EnsureIndex(wl => wl.Target.Id);
                WatchListCollection.EnsureIndex(wl => wl.Issuer.Id);
                WatchListCollection.EnsureIndex(wl => wl.Date);

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