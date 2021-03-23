namespace ModerationSystem
{
    using Events;
    using Exiled.API.Features;
    using System;

    public class Plugin : Plugin<Config>
    {

        public static Plugin Singleton;

        internal Events.Events Events{ get; private set; }
        public override string Author { get; } = "Twitch.tv/Monaldcry7788#9248";
        public override string Name { get; } = "ModerationSystem";
        public override Version Version { get; } = new Version(1, 1, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 8, 0);
        public bool WebhookEnabled { get; private set; } = false;

        public override void OnEnabled()
        {
            Singleton = this;

            Events = new Events.Events();
            Exiled.Events.Handlers.Player.Verified += Events.OnVerified;
            Exiled.Events.Handlers.Player.Destroying += Events.OnDestroying;
            
            Database.Open();
            if (Config.WebHookURL == "CHANGE ME")
            {
                Log.Error("Change Webhook URl in the Exiled config");
                WebhookEnabled = false;
            }
            try
            {
                Webhook.Http.sendMessage("**Server Connected!**", "ModerationSystem connected!");
                WebhookEnabled = true;
            }
            catch (Exception e)
            {
                Log.Error($"Error when i try to send webhook message:\n {e}");
                WebhookEnabled = false;
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Singleton = null;

            Exiled.Events.Handlers.Player.Verified -= Events.OnVerified;
            Exiled.Events.Handlers.Player.Destroying -= Events.OnDestroying;
            Events = null;

            Database.Close();

            base.OnDisabled();
        }
    }
}
