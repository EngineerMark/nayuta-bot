using System;
using Discord.WebSocket;

namespace nayuta.Commands
{
    public class CommandPing : Command
    {
        public CommandPing() : base("ping", "Test command to show network performance")
        {
        }

        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            return "Pong ("+ Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() -
                                      socketMessage.CreatedAt.ToUnixTimeMilliseconds())+"ms)";
        }
    }
}