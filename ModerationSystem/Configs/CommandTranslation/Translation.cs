namespace ModerationSystem.Configs.CommandTranslation
{
    using System.ComponentModel;

    public class Translation
    {
        [Description("Ban translation")]
        public BanTranslation BanTranslation { get; set; } = new BanTranslation();

        [Description("Kick translation")]
        public KickTranslation KickTranslation { get; set; } = new KickTranslation();

        [Description("Mute translation")]
        public MuteTranslation MuteTranslation { get; set; } = new MuteTranslation();

        [Description("Warn translatiob")]
        public WarnTranslation WarnTranslation { get; set; } = new WarnTranslation();

        [Description("SoftBan translation")]
        public SoftBanTranslation SoftBanTranslation { get; set; } = new SoftBanTranslation();

        [Description("Softwarn translation")]
        public SoftWarnTranslation SoftWarnTranslation { get; set; } = new SoftWarnTranslation();

        [Description("Clear translation")]
        public ClearTranslation ClearTranslation { get; set; } = new ClearTranslation();

        [Description("PlayerInfo translation")]
        public PlayerInfoTranslation PlayerInfoTranslation { get; set; } = new PlayerInfoTranslation();

        [Description("Staff translation")]
        public StaffTranslation StaffTranslation { get; set; } = new StaffTranslation();

        [Description("WatchList translation")]
        public WatchListTranslation WatchListTranslation { get; set; } = new WatchListTranslation();

        [Description("Discord translation")]
        public DiscordTranslation.DiscordTranslation DiscordTranslation { get; set; } = new DiscordTranslation.DiscordTranslation();

        [Description("Watchlist staff broadcast")]
        public Exiled.API.Features.Broadcast WatchlistStaffersBroadcastJoin { get; set; } = new Exiled.API.Features.Broadcast("<color=aqua>Player</color> <color=red>{player}</color> <color=aqua>è nella watchlist per:\n</color><color=red>{reason}</color>");
    }
}