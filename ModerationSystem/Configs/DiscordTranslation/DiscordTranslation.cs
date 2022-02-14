namespace ModerationSystem.Configs.DiscordTranslation
{
    using System.ComponentModel;

    public class DiscordTranslation
    {
        [Description("Webhook URL")]
        public string WebhookUrl { get; set; } = "https://discord.com/api/webhooks/";

        [Description("Webhook name")]
        public string WebhookName { get; set; } = "ModerationSystem";

        [Description("Webhook avatar_url")]
        public string Avatar { get; set; } = "https://i.imgur.com/SaqRzfU.png";

        [Description("Message content")]
        public string MessageContent { get; set; } = "```diff\n+ Player: {target}\n+ Reason: {reason}\n+ Action: {action}\n+ Duration: {duration}\n- Staffer: {issuer}\n```";

        [Description("Webhook for watchlist")]
        public string WebhookUrlWatchlist { get; set; } = "https://discord.com/api/webhooks/";

        [Description("Message content")]
        public string MessageContentWatchlist { get; set; } = "```diff\n+ Player: {target}\n+ Reason: {reason}\n- Staffer: {issuer}```";
    }
}