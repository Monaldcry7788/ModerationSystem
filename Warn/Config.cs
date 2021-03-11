using Exiled.API.Interfaces;
using System.ComponentModel;

namespace ModerationSystem
{
    public sealed class Config : IConfig
    {
        [Description("Enable or disable the plugin")]
        public bool IsEnabled { get; set; } = true;

        [Description("Enable or disable 'Creator' command")]
        public bool EnableCreatorCommand { get; set; } = true;

        [Description("The name of the database")]
        public string DatabaseName { get; private set; } = "Warns";

        [Description("The private broadcast when player get warned")]
        public Exiled.API.Features.Broadcast WarnMessage { get; private set; } = new Exiled.API.Features.Broadcast("<size=30><color=red>You have been warned!</color></size>\n<size=26><color=aqua>{reason}</color></size>", 10);

        [Description("The message when player will be kicked")]
        public string KickMessage { get; private set; } = "{reason}";

        [Description("The message when player will be banned")]
        public string BanMessage { get; private set; } = "{reason}";

        [Description("The private broadcast when player get muted")]
        public Exiled.API.Features.Broadcast MuteMessage { get; private set; } = new Exiled.API.Features.Broadcast("<size=30><color=red>You has been muted for </color><color=aqua>{duration}</color></size>\n<size=26><color=aqua>{reason}</color></size>!", 10);
    }
}
