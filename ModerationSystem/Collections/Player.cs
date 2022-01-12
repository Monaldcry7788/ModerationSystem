namespace ModerationSystem.Collections
{
    using System;
    using LiteDB;

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

        public bool IsMuted() => Database.Database.MuteCollection.Exists(mute => mute.Target.Id == Id && mute.Expire > DateTime.Now);

        public void Save() => Database.Database.PlayerCollection.Upsert(this);

        public bool IsBanned() => Database.Database.BanCollection.Exists(ban => ban.Target.Id == Id && ban.Expire > DateTime.Now);

        public bool IsSoftBanned() => Database.Database.SoftBanCollection.Exists(sb => sb.Target.Id == Id && sb.Expire > DateTime.Now);
    }
}