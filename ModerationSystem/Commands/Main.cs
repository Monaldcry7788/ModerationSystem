namespace ModerationSystem.Commands
{
    using System;
    using CommandSystem;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Main : ParentCommand
    {
        public Main() => LoadGeneratedCommands();

        public override string Command { get; } = "moderationsystem";

        public override string[] Aliases { get; } = { "ms" };

        public override string Description { get; } = string.Empty;

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(Mute.Instance);
            RegisterCommand(Warn.Instance);
            RegisterCommand(Kick.Instance);
            RegisterCommand(Ban.Instance);
            RegisterCommand(PlayerInfo.Instance);
            RegisterCommand(Clear.Instance);
            RegisterCommand(SoftBan.Instance);
            RegisterCommand(WatchList.Instance);
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Please, specify a sub command: warn, ban, kick, mute, playerinfo, clear, softwarn, softban, watchlist";
            return false;
        }
    }
}