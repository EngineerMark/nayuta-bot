using System;
using Discord.WebSocket;
using nayuta.Math;

namespace nayuta.Commands
{
    public class CommandPing : Command
    {
        public CommandPing() : base("ping", "Test command to show network performance")
        {
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            return "Pong ("+ Mathf.Abs(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() -
                                      socketMessage.CreatedAt.ToUnixTimeMilliseconds())+"ms)";
        }
    }
}