namespace ModerationSystem.Collections
{
    using System;
    using LiteDB;

    public class SoftBan
    {
        public SoftBan(Player target, Player issuer, string reason, string duration, DateTime date, DateTime expire, int softBanId, int server, bool clear)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
            SoftBanId = softBanId;
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

        public int SoftBanId { get; set; }

        public int Server { get; set; }

        public bool Clear { get; set; }

        public void Save() => Database.Database.SoftBanCollection.Insert(this);

        public void Update() => Database.Database.SoftBanCollection.Update(this);
    }
}