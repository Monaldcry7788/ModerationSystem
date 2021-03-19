namespace ModerationSystem.Commands
{
    using CommandSystem;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Main : ParentCommand
    {
        public Main() => LoadGeneratedCommands();

        public override string Command { get; } = "moderationsystem";

        public override string[] Aliases { get; } = new[] { "ms" };

        public override string Description { get; } = string.Empty;

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(Mute.Instance);
            RegisterCommand(Warn.Instance);
            RegisterCommand(Kick.Instance);
            RegisterCommand(Ban.Instance);
            RegisterCommand(Unwarn.Instance);
            RegisterCommand(Unban.Instance);
            RegisterCommand(Playerinfo.Instance);
            RegisterCommand(Unmute.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Please, specify a sub command: warn, ban, kick, mute, playerinfo, unmute, unwarn, unban";
            return false;
        }
    }
}