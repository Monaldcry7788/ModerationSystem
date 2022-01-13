namespace ModerationSystem.Collections
{
    using System;
    using LiteDB;

    public class Kick
    {
        public Kick(Player target, Player issuer, string reason, DateTime date, int kickId, int server, bool clear)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Date = date;
            KickId = kickId;
            Server = server;
            Clear = clear;
        }

        public ObjectId Id { get; set; }

        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }

        public int KickId { get; set; }

        public int Server { get; set; }

        public bool Clear { get; set; }

        public void Save() => Database.Database.KickCollection.Insert(this);

        public void Update() => Database.Database.KickCollection.Update(this);
    }
}