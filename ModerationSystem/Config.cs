namespace ModerationSystem
{
    using Exiled.API.Features;
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public class Config : IConfig
    {

        [Description("Enable or disable the plugin")]
        public bool IsEnabled { get; set; } = true;

        [Description("The name of the database")]
        public string DatabaseName { get; private set; } = "Warns";

        [Description("The private broadcast when player get warned")]
        public Exiled.API.Features.Broadcast WarnMessage { get; private set; } = new Broadcast("<size=30><color=red>You have been warned!</color></size>\n<size=26><color=aqua>{reason}</color></size>", 10);

        [Description("The message when player will be kicked")]
        public string KickMessage { get; private set; } = "{reason}";

        [Description("The message when player will be banned")]
        public string BanMessage { get; private set; } = "{reason}";

        [Description("The private broadcast when player get muted")]
        public Exiled.API.Features.Broadcast MuteMessage { get; private set; } = new Broadcast("<size=30><color=red>You has been muted for </color><color=aqua>{duration} minutes</color></size>\n<size=26><color=aqua>{reason}</color></size>", 10);

        [Description("Enable or disable auto kick")]
        public bool AutoKickEnable { get; private set; } = true;

        [Description("Maximum number of warns for the kick")]
        public int MaximumWarn { get; private set; } = 4;

        [Description("AutoKick message")]
        public string AutoKickMessage { get; private set; } = "Final warn: {reason}";

        [Description("WebHook URL")]
        public string WebHookURL { get; private set; } = "CHANGE ME";

        [Description("Name of WebHook")]
        public string WebHookName { get; private set; } = "ModerationSystem WebHook";

        [Description("The Message when player will be warned (Webhook)")]
        public string WarnedMessageWebHook { get; private set; } = "**Staffer:**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**Warnid:**\n {warnid}\n\n**Reason:**\n {reason}";

        [Description("The Message when player will be muted (Webhook)")]
        public string MutedMessageWebHook { get; private set; } = "**Staffer**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**MuteId:**\n {muteid}\n\n **Duration**\n{duration} minutes\n\n**Reason**\n{reason}";

        [Description("The Message when player will be banned (WebHook)")]
        public string BanMessageWebHook { get; private set; } = "**Staffer**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**BanId:**\n {banid}\n\n **Duration**\n{duration} minutes\n\n**Reason**\n{reason}";

        [Description("The Message when player will be kicked (Webhook)")]
        public string KickedMessageWebHook { get; private set; } = "**Staffer:**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**Kickid:**\n {kickid}\n\n**Reason:**\n {reason}";


    }
}
