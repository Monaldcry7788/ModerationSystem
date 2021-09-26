using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class Warn
    {
        public Warn()
        {
        }

        public Warn(Player target, Player issuer, string reason, DateTime date, int warnid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Date = date;
            Warnid = warnid;
        }

        public ObjectId Id { get; }

        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }
        public int Warnid { get; set; }

        public void Save() => Database.WarnCollection.Insert(this);

    }
}