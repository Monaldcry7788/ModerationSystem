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

        public Player Target { get; }

        public Player Issuer { get; }

        public string Reason { get; }

        public DateTime Date { get; }
        public int Kickid { get; }


        public void Save()
        {
            Database.LiteDatabase.GetCollection<Kick>().Insert(this);
        }
    }
}