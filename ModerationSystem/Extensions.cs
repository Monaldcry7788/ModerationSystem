using Player = Exiled.API.Features.Player;

namespace ModerationSystem
{
    using static Database;

    public static class Extensions
    {
        public static Collections.Player GetPlayer(this string player)
        {
            return Player.Get(player)?.GetPlayer() ??
                   LiteDatabase.GetCollection<Collections.Player>().Query()
                       .Where(qPlayer => qPlayer.Id == player.GetRawUserId() || qPlayer.Name == player)
                       .FirstOrDefault();
        }

        public static Collections.Player GetPlayer(this Player player)
        {
            if (player == null || string.IsNullOrEmpty(player.UserId) && !player.IsHost) return null;
            if (player.IsHost) return ServerPlayer;
            if (PlayerData.TryGetValue(player, out var dPlayer)) return dPlayer;
            return LiteDatabase.GetCollection<Collections.Player>().FindById(player.RawUserId);
        }
        private static string GetRawUserId(this string userId)
        {
            int index = userId.LastIndexOf('@');
            if (index == -1) return userId;
            return userId.Substring(0, index);
        }
        public static Collections.Player GetStaffer(this CommandSender sender)
        {
            return new Collections.Player
            (
                sender?.SenderId?.GetRawUserId() ?? "Server",
                sender?.SenderId?.GetAuthentication() ?? "Server",
                sender?.Nickname ?? "Server",
                false
            );
        }
        private static string GetAuthentication(this string userId) => userId.Substring(userId.LastIndexOf('@') + 1);
        }
}