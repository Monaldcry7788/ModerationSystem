namespace ModerationSystem.Configs.CommandTranslation
{
    using System.ComponentModel;

    public class SoftBanTranslation
    {
        [Description("The private message when player will be softbanned")]
        public Exiled.API.Features.Broadcast PlayerSoftBanMessage { get; set; } = new Exiled.API.Features.Broadcast("You has been softbanned for {duration} with reason: {reason}", 10);

        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms softban/sb <player name or ID> <duration(HH:mm:ss)> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Invalid duration error")]
        public string InvalidDuration { get; set; } = "Invalid duration {duration}";

        [Description("Reason null error")]
        public string ReasonNull { get; set; } = "Reason can't be null";

        [Description("Player already soft-banned")]
        public string PlayerAlreadySoftBanned { get; set; } = "Player is already soft-banned";

        [Description("Player succesfully soft-banned")]
        public string PlayerSoftBanned { get; set; } = "Player {player.name} ({player.userid}) has been softbanned for {duration} with reason {reason}";
    }
}