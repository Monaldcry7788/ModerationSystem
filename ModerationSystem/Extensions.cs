namespace ModerationSystem
{
    using Exiled.API.Features;
    using System;
    using static Database;

    public static class Extensions
    {

        public static bool IsChatMuted(this Player player)
        {
            return LiteDatabase.GetCollection<Collections.Mute>().Exists(mute => mute.Target.Id == player.UserId && mute.Expire > DateTime.Now);
        }

        public static Collections.Player GetPlayer(this string player)
        {
            return Player.Get(player)?.GetPlayer() ??
                LiteDatabase.GetCollection<Collections.Player>().Query().Where(qPlayer => qPlayer.Id == player.GetRawUserId() || qPlayer.Name == player).FirstOrDefault();
        }
        public static Collections.Player GetPlayer(this Player player)
        {
            if (player == null || (string.IsNullOrEmpty(player.UserId) && !player.IsHost))
                return null;
            else if (player.IsHost)
                return ServerPlayer;
            else if (PlayerData.TryGetValue(player, out Collections.Player dPlayer))
                return dPlayer;
            else
                return LiteDatabase.GetCollection<Collections.Player>().FindById(player.RawUserId);
        }

        public static Collections.Player GetStaffer(this CommandSender sender) => new Collections.Player
        (
            sender?.SenderId?.GetRawUserId() ?? "Server",
            sender?.SenderId?.GetAuthentication() ?? "Server",
            sender?.Nickname ?? "Server"
        );

        public static string GetAuthentication(this string userId) => userId.Substring(userId.LastIndexOf('@') + 1);

        public static string GetRawUserId(this string userId)
        {
            int index = userId.LastIndexOf('@');

            if (index == -1)
                return userId;

            return userId.Substring(0, index);
        }
    }
}
