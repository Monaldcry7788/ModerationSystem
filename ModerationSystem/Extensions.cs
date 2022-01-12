namespace ModerationSystem
{
    using Player = Exiled.API.Features.Player;
    using static Database.Database;

    public static class Extensions
    {
        internal static Collections.Player GetPlayer(this string player) => Player.Get(player)?.GetPlayer() ?? PlayerCollection.Query().Where(qPlayer => qPlayer.Id == player.GetRawUserId() || qPlayer.Name == player).FirstOrDefault();

        internal static Collections.Player GetPlayer(this Player player)
        {
            if (player == null || string.IsNullOrEmpty(player.UserId) && !player.IsHost) return null;

            if (player.IsHost) return ServerPlayer;

            if (PlayerData.TryGetValue(player, out Collections.Player dPlayer)) return dPlayer;

            return PlayerCollection.FindById(player.RawUserId);
        }

        private static string GetRawUserId(this string userId)
        {
            int index = userId.LastIndexOf('@');
            return index == -1 ? userId : userId.Substring(0, index);
        }

        internal static Collections.Player GetStaffer(this CommandSender sender)
        {
            return new Collections.Player
            (
                sender?.SenderId?.GetRawUserId() ?? "Server",
                sender?.SenderId?.GetAuthentication() ?? "Server",
                sender?.Nickname ?? "Server"
            );
        }

        private static string GetAuthentication(this string userId) => userId.Substring(userId.LastIndexOf('@') + 1);
    }
}