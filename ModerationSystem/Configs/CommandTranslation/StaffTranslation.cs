using System.ComponentModel;

namespace ModerationSystem.Configs.CommandTranslation
{
    public class StaffTranslation
    {
        [Description("The broadcast to the staff when player get warned")]
        public Exiled.API.Features.Broadcast StaffWarnMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=aqua>Staffer</color> <color=red>{staffer}</color> <color=aqua>warned</color> <color=red>{target}</color> <color=aqua>for:</color></size>\n<color=aqua>{reason}</color>");
        
        [Description("The broadcast to the staff when player get kicked")]
        public Exiled.API.Features.Broadcast StaffKickMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=aqua>Staffer</color> <color=red>{staffer}</color> <color=aqua>kicked</color> <color=red>{target}</color> <color=aqua>for:</color></size>\n<color=aqua>{reason}</color>");
        
        [Description("The broadcast to the staff when player get muted")]
        public Exiled.API.Features.Broadcast StaffMuteMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=aqua>Staffer</color> <color=red>{staffer}</color> <color=aqua>muted</color> <color=red>{target}</color> <color=aqua>for</color> <color=red>{time}</color> <color=aqua>with reason:</color></size>\n<color=aqua>{reason}</color>");

        [Description("The broadcast to the staff when player get banned")]
        public Exiled.API.Features.Broadcast StaffBanMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=aqua>Staffer</color> <color=red>{staffer}</color> <color=aqua>banned</color> <color=red>{target}</color> <color=aqua>for</color> <color=red>{time}</color> <color=aqua>with reason:</color></size>\n<color=aqua>{reason}</color>");
        
        [Description("The private broadcast when player get softwarned")]
        public Exiled.API.Features.Broadcast StaffSoftWarnMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=aqua>Staffer</color> <color=red>{staffer}</color> <color=aqua>softwarned</color> <color=red>{target}</color> <color=aqua>for:</color></size>\n<color=aqua>{reason}</color>");
        
        [Description("The private broadcast when player get softbanned")]
        public Exiled.API.Features.Broadcast StaffSoftBanMessage { get; set; } = new Exiled.API.Features.Broadcast("<size=30><color=aqua>Staffer</color> <color=red>{staffer}</color> <color=aqua>softbanned</color> <color=red>{target}</color> <color=aqua>for</color> <color=red>{time}</color> <color=aqua>with reason:</color></size>\n<color=aqua>{reason}</color>");
    }
}