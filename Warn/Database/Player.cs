namespace ModerationSystem
{
    using LiteDB;

    public class Player
    {
        public string Id { get; }
        public string Name { get; internal set; }
        public string Auth { get; }

        [BsonCtor]
        public Player(string id, string auth, string name)
        {
            Id = id;
            Auth = auth;
            Name = name;
        }
        public void Save() => Database.LiteDatabase.GetCollection<Player>().Upsert(this);

    }
}
