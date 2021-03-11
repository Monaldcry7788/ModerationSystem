using LiteDB;

namespace ModerationSystem
{
    public class MuteSystem
    {
        public MuteSystem()
        {
        }
        public ObjectId Id { get; private set; }
        public Player Target { get; private set; }
        public Player Issuer { get; private set; }
        public string Reason { get; private set; }
        public int MuteId { get; private set; }
        public string Expire { get; private set; }
        public string Date { get; private set; }
        public int Duration { get; private set; }

        public MuteSystem(Player target, Player issuer, string reason, int duration, string date, string expire, int muteid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
            MuteId = muteid;

        }
        public void Save() => Database.LiteDatabase.GetCollection<MuteSystem>().Insert(this);

    }
}
