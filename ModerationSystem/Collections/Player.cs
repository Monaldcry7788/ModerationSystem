using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Player
    {
        [BsonCtor]
        public Player(string id, string authentication, string name, bool isactuallymuted)
        {
            Id = id;
            Authentication = authentication;
            Name = name;
            IsActuallyMuted = isactuallymuted;
        }

        public string Id { get; }

        public string Authentication { get; }

        public string Name { get; internal set; }
        public bool IsActuallyMuted { get; internal set; }


        public bool IsMuted()
        {
            return Database.LiteDatabase.GetCollection<Mute>().Exists(mute => mute.Target.Id == Id && mute.Expire > DateTime.Now);
        }

        public void Save() => Database.LiteDatabase.GetCollection<Player>().Upsert(this);

            public bool IsBanned()
        {
            return Database.LiteDatabase.GetCollection<Ban>().Exists(ban => ban.Target.Id == Id && ban.Expire > DateTime.Now);
        }
    }
}