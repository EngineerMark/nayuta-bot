using Discord.WebSocket;

namespace nayuta.Commands
{
    public abstract class CommandOsu : Command
    {
        public CommandOsu(string command, string description = null) : base("osu"+command, description)
        {
        }

        public override abstract object CommandHandler(SocketMessage socketMessage, string input);
    }
}