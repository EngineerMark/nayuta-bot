using Discord.WebSocket;

namespace nayuta.Commands
{
    public class CommandPing : Command
    {
        public CommandPing() : base("ping", null)
        {
            returnFunc = (SocketMessage socketMessage, string input) =>
            {
                return "Pong";
            };
        }
    }
}