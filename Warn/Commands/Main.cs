using CommandSystem;
using System;

namespace ModerationSystem
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]

    public class Main : ParentCommand
    {
        public Main() => LoadGeneratedCommands();
        public override string Command { get; } = "ModerationSystem";
        public override string[] Aliases { get; } = new[] { "ms", "m", "w" };
        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(Warn.Instance);
            RegisterCommand(PlayerInfo.Instance);
            RegisterCommand(Kick.Instance);
            RegisterCommand(Ban.Instance);
            RegisterCommand(Mute.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = string.Format("Please, specify a sub command: warn, ban, kick, mute, playerinfo");
            return false;
        }
    }
}
