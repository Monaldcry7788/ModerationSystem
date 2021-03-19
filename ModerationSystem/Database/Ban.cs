using LiteDB;
using System;

namespace ModerationSystem.Collections
{
    public class Ban
    {
        public Ban()
        {
        }

        public Ban(Player target, Player issuer, string reason, double duration, DateTime date, DateTime expire, int banid)
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

        public ObjectId Id { get; private set; }

        public Player Target { get; private set; }

        public Player Issuer { get; private set; }

        public string Reason { get; private set; }

        public double Duration { get; private set; }

        public DateTime Date { get; private set; }

        public DateTime Expire { get; private set; }
        public int Banid { get; private set; }

        public void Save() => Database.LiteDatabase.GetCollection<Ban>().Insert(this);
    }
}
