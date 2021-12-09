using System.ComponentModel;
using ModerationSystem.Configs.CommandTranslation;

namespace ModerationSystem.Configs
{
    public class Translation
    {
        [Description("Ban translation")] public BanTranslation BanTranslation { get; set; } = new BanTranslation();
        [Description("Kick translation")] public KickTranslation KickTranslation { get; set; } = new KickTranslation();
        [Description("Mute translation")] public MuteTranslation MuteTranslation { get; set; } = new MuteTranslation();
        [Description("Warn translatiob")] public WarnTranslation WarnTranslation { get; set; } = new WarnTranslation();

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
    }
}