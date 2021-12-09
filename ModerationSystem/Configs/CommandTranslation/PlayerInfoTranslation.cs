using System.ComponentModel;

namespace ModerationSystem.Configs.CommandTranslation
{
    public class PlayerInfoTranslation
    {
        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms playerinfo/pi <player name or ID>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";
    }
}