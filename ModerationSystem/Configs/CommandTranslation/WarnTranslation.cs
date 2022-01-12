namespace ModerationSystem.Configs.CommandTranslation
{
    using System.ComponentModel;

    public class WarnTranslation
    {
        [Description("The private message when player will be warned")]
        public Exiled.API.Features.Broadcast PlayerWarnedMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=red>You have been warned!</color></size>\n<size=26><color=aqua>{reason}</color></size>", 10);

        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms warn/w <player name or ID> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Reason null error")] public string ReasonNull { get; set; } = "Reason can't be null";
        
        [Description("Player succesfully warned")]
        public string PlayerWarned { get; set; } = "Player {player.name} ({player.userid}) has been warned with reason {reason}";
        
    }
}