namespace ModerationSystem.Configs.CommandTranslation
{
    using System.ComponentModel;

    public class WatchListTranslation
    {
        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: aw watchlist <add / remove> <player id / userid / nickname> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Message when player will be added into watchlist")]
        public string PlayerAddedWatchlist { get; set; } = "Player {player.name} ({player.userid}) succesfully added into whitelist";

        [Description("Action not founded error")]
        public string ActionNotFounded { get; set; } = "Action not founded! Avariable action: add, remove";
        [Description("Reason null error")] public string ReasonNull { get; set; } = "Reason can't be null";

    }
}