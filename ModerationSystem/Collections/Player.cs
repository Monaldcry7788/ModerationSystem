namespace ModerationSystem.Collections
{
    using System;
    using LiteDB;

    public class Player
    {
        public Player()
        {
        }

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

        public void Save() => Database.Database.PlayerCollection.Upsert(this);
    }
}