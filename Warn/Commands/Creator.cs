using CommandSystem;
using System;
using Features = Exiled.API.Features;
namespace ModerationSystem
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Creator : ICommand
    {
        public string Command { get; } = "WarnSystemTag";
        public string[] Aliases { get; } = new[] { "wtag", "warntag" };
        public string Description { get; } = "Get tag";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            bool AlreadyTag = false;
            var monald = Features.Player.Get((CommandSender)sender);
            if (!Plugin.Singleton.Config.EnableCreatorCommand)
            {
                response = "<color=red>Command is disable by server config</color>";
                return false;
            }
            if (!monald.UserId.Contains("76561198406583817@steam"))
            {
                response = "<color=red>You can't do this command because you isn't plugin creator!</color>";
                return false;
            }
            if (arguments.Count != 0)
            {
                response = "";
                return false;
            }
            if (AlreadyTag == false)
            {
                Extensions.SetRank(monald, "ModerationSystem Developer", "magenta");
                response = "<color=green>Tag show!</color>";
                AlreadyTag = true;
                return true;
            }
            Extensions.SetRank(monald, "", "default");
            AlreadyTag = false;
            response = "<color=green>Tag hidden, type *tag* for refresh your tag</color>";
            return true;
        }
    }
}
