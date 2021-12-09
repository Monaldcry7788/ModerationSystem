using System.ComponentModel;

namespace ModerationSystem.Configs.CommandTranslation
{
    public class MuteTranslation
    {
        [Description("The private message when player will be muted")]
        public Exiled.API.Features.Broadcast PlayerMuteMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=red>You has been muted for </color><color=aqua>{duration}</color></size>\n<size=26><color=aqua>{reason}</color></size>", 10);

        [Description("Invalid permission")]
        public string InvalidPermission { get; set; } = "You can't do this command. Required permission {permission}";

        [Description("Wrong usage")]
        public string WrongUsage { get; set; } = "Invalid usage. Usage: ms mute/m <player name or ID> <time(HH:mm:ss)> <reason>";

        [Description("Player not found error")]
        public string PlayerNotFound { get; set; } = "Player not found";

        [Description("Invalid duration error")]
        public string InvalidDuration { get; set; } = "Invalid duration {duration}";

        [Description("Reason null error")] public string ReasonNull { get; set; } = "Reason can't be null";

        [Description("Player already muted")] public string PlayerAlreadyMuted { get; set; } = "Player is already muted";

        [Description("Player succesfully banned")]
        public string PlayerMuted { get; set; } = "Player {player.name} ({player.userid}) has been muted for {duration} with reason {reason}";

    }
}