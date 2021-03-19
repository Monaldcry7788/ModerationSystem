using LiteDB;
using System;

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

        public ObjectId Id { get; private set; }

        public Player Target { get; private set; }

        public Player Issuer { get; private set; }

        public string Reason { get; private set; }

        public DateTime Date { get; private set; }
        public int Warnid { get; private set; }

        public void Save() => Database.LiteDatabase.GetCollection<Warn>().Insert(this);
    }
}
