using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Warn
    {
        public Warn()
        {
        }

        public Warn(Player target, Player issuer, string reason, DateTime date, int warnId, int server, bool clear)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Date = date;
            WarnId = warnId;
            Server = server;
            Clear = clear;
        }

        public ObjectId Id { get; set; }
        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }
        public int WarnId { get; set; }
        public int Server { get; set; }
        public bool Clear { get; set; }

        public void Save() => Database.WarnCollection.Insert(this);
        public void Update() => Database.WarnCollection.Update(this);

    }
}