using System;
using LiteDB;

namespace ModerationSystem.Collections
{
    public class WatchList
    {
        public WatchList()
            {
            }

            public WatchList(Player target, Player issuer, string reason, DateTime date, int watchListId, int server, bool clear)
            {
                Id = ObjectId.NewObjectId();
                Target = target;
                Issuer = issuer;
                Reason = reason;
                Date = date;
                WatchListId = watchListId;
                Server = server;
                Clear = clear;
            }

            public ObjectId Id { get; set; }
            public Player Target { get; set; }

            public Player Issuer { get; set; }

            public string Reason { get; set; }

            public DateTime Date { get; set; }
            public int WatchListId { get; set; }
            public int Server { get; set; }
            public bool Clear { get; set; }

            public void Save() => Database.WatchListCollection.Insert(this);
            public void Update() => Database.WatchListCollection.Update(this);
    }
}