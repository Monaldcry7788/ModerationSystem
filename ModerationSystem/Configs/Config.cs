namespace ModerationSystem.Configs
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Exiled.API.Interfaces;
    using ModerationSystem.Configs.CommandTranslation;

    public class Config : IConfig
    {
        [Description("Enable or disable the plugin")]
        public bool IsEnabled { get; set; } = true;

        [Description("The name of the database")]
        public string DatabaseName { get; set; } = "ModerationSystem";

        [Description("Is database global")]
        public bool IsDatabaseGlobal { get; set; } = false;

        [Description("List of server ports to receive data from")]
        public List<string> ReceiveFrom { get; set; } = new List<string>
        {
            "7778",
            "7779"
        };

        [Description("Plugin translation")]
        public Translation Translation { get; set; } = new Translation();

        [Description("Staffer max duration permission (in minute)")]
        public Dictionary<string, int> StaffersDurationPermission { get; set; } = new()
        {
            {
                "ms.duration.60", 60
            }
        };
    }
}