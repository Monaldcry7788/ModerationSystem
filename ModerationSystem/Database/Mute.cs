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

        public ObjectId Id { get; set; }

        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public double Duration { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expire { get; set; }
        public int Muteid { get; set; }

        public void Save()
        {
            Database.LiteDatabase.GetCollection<Mute>().Insert(this);
        }
    }
}