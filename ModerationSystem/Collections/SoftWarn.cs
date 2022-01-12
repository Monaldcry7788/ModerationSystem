namespace ModerationSystem.Collections
{
    using System;
    using LiteDB;

    public class SoftWarn
    {
        public SoftWarn(Player target, Player issuer, string reason, DateTime date, int softWarnId, int server, bool clear)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Date = date;
            SoftWarnId = softWarnId;
            Server = server;
            Clear = clear;
        }
        public ObjectId Id { get; set; }

        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }

        public int SoftWarnId { get; set; }

        public int Server { get; set; }

        public bool Clear { get; set; }

        public void Save() => Database.Database.SoftWarnCollection.Insert(this);

        public void Update() => Database.Database.SoftWarnCollection.Update(this);

    }
}