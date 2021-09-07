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

        public Player Target { get; }

        public Player Issuer { get; }

        public string Reason { get; }

        public DateTime Date { get; }
        public int Warnid { get; }

        public void Save()
        {
            Database.LiteDatabase.GetCollection<Warn>().Insert(this);
        }
    }
}