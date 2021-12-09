﻿using System;
using LiteDB;

namespace ModerationSystem.Collections
{
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


        public bool IsMuted() => Database.MuteCollection.Exists(mute => mute.Target.Id == Id && mute.Expire > DateTime.Now);

        public void Save() => Database.PlayerCollection.Upsert(this);

        public bool IsBanned() => Database.BanCollection.Exists(ban => ban.Target.Id == Id && ban.Expire > DateTime.Now);

        public bool IsSoftBanned() => Database.SoftBanCollection.Exists(sb => sb.Target.Id == Id && sb.Expire > DateTime.Now);
    }
}