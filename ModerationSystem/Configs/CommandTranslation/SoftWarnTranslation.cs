namespace ModerationSystem.Configs.CommandTranslation
{
    using System.ComponentModel;

    public class SoftWarnTranslation
    {
        [Description("The private message when player will be softwarned")]
        public Exiled.API.Features.Broadcast PlayerSoftWarnedMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=red>You have been softwarned!</color></size>\n<size=26><color=aqua>{reason}</color></size>", 10);

        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms softwarn/sw <player name or ID> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Reason null error")] public string ReasonNull { get; set; } = "Reason can't be null";
        
        [Description("Player succesfully softwarned")]
        public string PlayerSoftWarned { get; set; } = "Player {player.name} ({player.userid}) has been softwarned with reason {reason}";
    }
}