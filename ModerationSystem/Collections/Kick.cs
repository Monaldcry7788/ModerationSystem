using System;
using LiteDB;

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

        public ObjectId Id { get; }

        public Player Target { get; set; }

        public Player Issuer { get; set; }

        public string Reason { get; set; }

        public DateTime Date { get; set; }
        public int Kickid { get; set; }


        public void Save() => Database.KickCollection.Insert(this);
    }
}