namespace ModerationSystem.Collections
{
    using LiteDB;
    using System;

    public class Mute
    {
        public Mute()
        {
        }

        public Mute(Player target, Player issuer, string reason, double duration, DateTime date, DateTime expire, int muteid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
        }

        public ObjectId Id { get; private set; }

        public Player Target { get; private set; }

        public Player Issuer { get; private set; }

        public string Reason { get; private set; }

        public double Duration { get; private set; }

        public DateTime Date { get; private set; }

        public DateTime Expire { get; private set; }
        public int Muteid { get; private set; }

        public void Save() => Database.LiteDatabase.GetCollection<Mute>().Insert(this);
    }
}
