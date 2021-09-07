using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Ban
    {
        public Ban()
        {
        }

        public Ban(Player target, Player issuer, string reason, double duration, DateTime date, DateTime expire,
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

        public Player Target { get; }

        public Player Issuer { get; }

        public string Reason { get; }

        public double Duration { get; }

        public DateTime Date { get; }

        public DateTime Expire { get; }
        public int Banid { get; }

        public void Save()
        {
            Database.LiteDatabase.GetCollection<Ban>().Insert(this);
        }
    }
}