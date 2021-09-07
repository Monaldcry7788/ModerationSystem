using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Mute
    {
        public Mute()
        {
        }

        public Mute(Player target, Player issuer, string reason, double duration, DateTime date, DateTime expire,
            int muteid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
        }

        public ObjectId Id { get; }

        public Player Target { get; }

        public Player Issuer { get; }

        public string Reason { get; }

        public double Duration { get; }

        public DateTime Date { get; }

        public DateTime Expire { get; }
        public int Muteid { get; private set; }

        public void Save()
        {
            Database.LiteDatabase.GetCollection<Mute>().Insert(this);
        }
    }
}