namespace ModerationSystem.Discord.Discord
{
    public class Message
    {
        public Message(string content)
        {
            username = Plugin.Singleton.Config.Translation.DiscordTranslation.WebhookName;
            avatar_url = Plugin.Singleton.Config.Translation.DiscordTranslation.Avatar;
            this.content = content;
        }

        public string username { get; }

        public string avatar_url { get; }

        public string content { get; }
    }
}