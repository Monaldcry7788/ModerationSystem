using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Ban
    {
        public Ban()
        {
        }

        public Ban(Player target, Player issuer, string reason, string duration, DateTime date, DateTime expire,
            int banid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
            Banid = banid;
        }

        public ObjectId Id { get; }

        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public string Duration { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expire { get; set; }
        public int Banid { get; set; }

        public void Save()
        {
            Database.BanCollection.Insert(this);
        }
    }
}