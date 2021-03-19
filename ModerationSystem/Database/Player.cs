namespace ModerationSystem.Collections
{
    using LiteDB;
    using System;

    public class Player
    {
        [BsonCtor]
        public Player(string id, string authentication, string name)
        {
            Id = id;
            Authentication = authentication;
            Name = name;
        }

        public string Id { get; }

        public string Authentication { get; }

        public string Name { get; internal set; }


        public bool IsMuted() => Database.LiteDatabase.GetCollection<Mute>().Exists(mute => mute.Target.Id == Id && mute.Expire > DateTime.Now);

        public void Save() => Database.LiteDatabase.GetCollection<Player>().Upsert(this);
        public bool IsBanned() => Database.LiteDatabase.GetCollection<Ban>().Exists(ban => ban.Target.Id == Id && ban.Expire > DateTime.Now);
    }
}
