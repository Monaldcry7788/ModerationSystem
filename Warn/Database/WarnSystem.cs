using LiteDB;

namespace ModerationSystem
{
    public class WarnSystem
    {
        public WarnSystem()
        {
        }
        public ObjectId Id { get; private set; }
        public Player Target { get; private set; }
        public Player Issuer { get; private set; }
        public string Reason { get; private set; }
        public int WarnId { get; private set; }
        public string Date { get; private set; }
        public WarnSystem(Player target, Player issuer, string reason, string date, int warnid)
        {
            Id = ObjectId.NewObjectId();
            Target = target;
            Issuer = issuer;
            Reason = reason;
            WarnId = warnid;
            Date = date;
        }
        public void Save() => Database.LiteDatabase.GetCollection<WarnSystem>().Insert(this);
    }
}
