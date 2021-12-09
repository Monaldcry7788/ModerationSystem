using System.ComponentModel;

namespace ModerationSystem.Configs.CommandTranslation
{
    public class ClearTranslation
    {
        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: aw clear <player id / userid / nickname> <all (for clear everything)> <ban/kick/mute/softban/softwarn/warn> <id> <server-port>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Message when player will be cleared")]
        public string PlayerCleared { get; set; } = "Player {player.name} ({player.userid}) cleared";
        [Description("Id not found error")] public string IdNotFound { get; set; } = "Id not found";

        [Description("Punishment cleared from player")]
        public string PunishmentCleared { get; set; } = "Punishment cleared from {player.name} ({player.userid})";

        [Description("Action not founded error")]
        public string ActionNotFounded { get; set; } =
            "Action not founded! Avariable action: ban, kickm mute, warn, softban, softwarn";
    }
}