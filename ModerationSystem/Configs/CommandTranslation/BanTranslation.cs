using System.ComponentModel;

namespace ModerationSystem.Configs.CommandTranslation
{
    public class BanTranslation
    {
        [Description("The private message when player will be banned")]
        public string PlayerBanMessage { get; set; } = "{reason}";

        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms ban/b <player name or ID> <duration(HH:mm:ss)> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Invalid duration error")]
        public string InvalidDuration { get; set; } = "Invalid duration {duration}";

        [Description("Reason null error")] public string ReasonNull { get; set; } = "Reason can't be null";

        [Description("Player already banned")] public string PlayerAlreadyBanned { get; set; } = "Player is already banned";

        [Description("Player succesfully banned")]
        public string PlayerBanned { get; set; } = "Player {player.name} ({player.userid}) has been banned for {duration} with reason {reason}";
        

    }
}