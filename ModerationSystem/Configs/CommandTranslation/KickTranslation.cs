namespace ModerationSystem.Configs.CommandTranslation
{
    using System.ComponentModel;

    public class KickTranslation
    {
        [Description("The private message when player will be kicked")]
        public string PlayerKickedMessage { get; set; } = "{reason}";

        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms kick/k <player name or ID> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Reason null error")] public string ReasonNull { get; set; } = "Reason can't be null";
        
        [Description("Player succesfully kicked")]
        public string PlayerKicked { get; set; } = "Player {player.name} ({player.userid}) has been kicked with reason {reason}";

    }
}