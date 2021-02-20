using Discord.WebSocket;

namespace nayuta
{
    public interface ICommand
    {
        object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments);
    }
}