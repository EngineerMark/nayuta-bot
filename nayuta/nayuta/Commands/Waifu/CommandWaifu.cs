using Discord.WebSocket;

namespace nayuta.Commands.Waifu
{
    public abstract class CommandWaifu : Command
    {
        public CommandWaifu(string commandName, string commandDescription = null) : base("waifu"+commandName, commandDescription)
        {
        }
        
        public abstract override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments);
    }
}