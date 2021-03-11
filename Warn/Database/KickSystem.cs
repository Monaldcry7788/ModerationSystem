using LiteDB;


namespace ModerationSystem
{
    public class KickSystem
    {
        public KickSystem()
        {
        }
        public ObjectId Id { get; private set; }
        public Player Target { get; private set; }
        public Player Issuer { get; private set; }
        public string Reason { get; private set; }
        public int KickId { get; private set; }
        public string Date { get; private set; }

        public KickSystem(Player target, Player issuer, string reason, string date, int kickid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            KickId = kickid;
            Date = date;
        }
        public void Save() => Database.LiteDatabase.GetCollection<KickSystem>().Insert(this);
    }
}
