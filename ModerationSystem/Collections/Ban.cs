using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Ban
    {
        public Ban(Player target, Player issuer, string reason, string duration, DateTime date, DateTime expire,
            int banId, int server, bool clear)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
            BanId = banId;
            Server = server;
            Clear = clear;
        }
        public ObjectId Id { get; set; }
        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public string Duration { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expire { get; set; }
        public int BanId { get; set; }
        public int Server { get; set; }
        public bool Clear { get; set; }

        public void Save() => Database.BanCollection.Insert(this);
        public void Update() => Database.BanCollection.Update(this);
    }
}