using LiteDB;
using System;

namespace ModerationSystem.Collections
{
    public class Kick
    {
        public Kick()
        {
        }

        public Kick(Player target, Player issuer, string reason, DateTime date, int kickid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Date = date;
            Kickid = kickid;
        }

        public ObjectId Id { get; private set; }

        public Player Target { get; private set; }

        public Player Issuer { get; private set; }

        public string Reason { get; private set; }

        public DateTime Date { get; private set; }
        public int Kickid { get; private set; }


        public void Save() => Database.LiteDatabase.GetCollection<Kick>().Insert(this);
    }
}
