namespace ModerationSystem
{
    public static class Extensions
    {
        public static Player GetPlayer(this string player)
        {
            return Exiled.API.Features.Player.Get(player)?.GetPlayer() ??
                Database.LiteDatabase.GetCollection<Player>().Query().Where(qPlayer => qPlayer.Id == player.GetRawId() || qPlayer.Name == player).FirstOrDefault();
        }

        public static Player GetPlayer(this Exiled.API.Features.Player player)
        {
            if (player == null || (string.IsNullOrEmpty(player.UserId)))
                return null;
            else if (Database.PlayerData.TryGetValue(player, out Player dPlayer))
                return dPlayer;
            else
                return Database.LiteDatabase.GetCollection<Player>().FindById(player.RawUserId);
        }

        public static Player GetStaff(this CommandSender sender) => new Player
        (
            sender?.SenderId?.GetRawId() ?? "Server",
            sender?.SenderId?.GetAuth() ?? "Server",
            sender?.Nickname ?? "Server"
        );

        public static string GetAuth(this string userId) => userId.Substring(userId.LastIndexOf('@') + 1);

        public static string GetRawId(this string userId)
        {
            int index = userId.LastIndexOf('@');
            if (index == -1)
                return userId;
            return userId.Substring(0, index);
        }

        public static void SetRank(this Exiled.API.Features.Player player, string rank, string color = "default")
        {
            player.ReferenceHub.serverRoles.NetworkMyText = rank;
            player.ReferenceHub.serverRoles.NetworkMyColor = color;
        }
    }
}
