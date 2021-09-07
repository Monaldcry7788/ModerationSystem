using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace ModerationSystem
{
    public class Config : IConfig
    {
        [Description("The name of the database")]
        public string DatabaseName { get; } = "Warns";

        [Description("The private broadcast when player get warned")]
        public Exiled.API.Features.Broadcast WarnMessage { get; } = new Exiled.API.Features.Broadcast(
            "<size=30><color=red>You have been warned!</color></size>\n<size=26><color=aqua>{reason}</color></size>");

        [Description("The message when player will be kicked")]
        public string KickMessage { get; } = "{reason}";

        [Description("The message when player will be banned")]
        public string BanMessage { get; } = "{reason}";

        [Description("The private broadcast when player get muted")]
        public Exiled.API.Features.Broadcast MuteMessage { get; } = new Exiled.API.Features.Broadcast(
            "<size=30><color=red>You has been muted for </color><color=aqua>{duration} minutes</color></size>\n<size=26><color=aqua>{reason}</color></size>");

        [Description("WebHook URL")] public string WebHookURL { get; } = "CHANGE ME";

        [Description("Name of WebHook")] public string WebHookName { get; } = "ModerationSystem WebHook";

        [Description("The Message when player will be warned (Webhook)")]
        public string WarnedMessageWebHook { get; } =
            "**Staffer:**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**Warnid:**\n {warnid}\n\n**Reason:**\n {reason}";

        [Description("The Message when player will be muted (Webhook)")]
        public string MutedMessageWebHook { get; } =
            "**Staffer**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**MuteId:**\n {muteid}\n\n **Duration**\n{duration} minutes\n\n**Reason**\n{reason}";

        [Description("The Message when player will be banned (WebHook)")]
        public string BanMessageWebHook { get; } =
            "**Staffer**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**BanId:**\n {banid}\n\n **Duration**\n{duration} minutes\n\n**Reason**\n{reason}";

        [Description("The Message when player will be kicked (Webhook)")]
        public string KickedMessageWebHook { get; } =
            "**Staffer:**\n {staffer}\n\n**Target:**\n {target.Name} {target.Id}\n\n**Kickid:**\n {kickid}\n\n**Reason:**\n {reason}";

        [Description("Enable or disable anti team kill")]
        public bool IsAntiTeamKillEnabled { get; } = true;

        [Description("Number of peaple for invoke reverse team kill")]
        public int ReverseTeamKillNumber { get; } = 2;

        [Description("Action for reverse team kill: nothing, warn, kick, ban")]
        public string Action { get; set; } = "kick";

        [Description("Action reason for reverse team kill")]
        public Dictionary<string, string> ActionReason { get; set; } = new Dictionary<string, string>
        {
            {
                "warn", "<color=red>You has been warned!</color>\n<color=yellow>TeamKill</color>"
            }
        };

        [Description("Ban duration for teamkill (in minutes)")]
        public int BanDuration { get; set; } = 60;

        [Description("Enable or disable the plugin")]
        public bool IsEnabled { get; set; } = true;
    }
}