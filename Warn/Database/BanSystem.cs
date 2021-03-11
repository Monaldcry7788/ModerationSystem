using LiteDB;

namespace ModerationSystem
{
    public class BanSystem
    {
        public BanSystem()
        {
        }
        public ObjectId Id { get; private set; }
        public Player Target { get; private set; }
        public Player Issuer { get; private set; }
        public string Reason { get; private set; }
        public int BanId { get; private set; }
        public string Expire { get; private set; }
        public string Date { get; private set; }
        public int Duration { get; private set; }

        public BanSystem(Player target, Player issuer, string reason, int duration, string date, string expire, int banid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            Duration = duration;
            Date = date;
            Expire = expire;
            BanId = banid;

        }
        public void Save() => Database.LiteDatabase.GetCollection<BanSystem>().Insert(this);
    }
}
