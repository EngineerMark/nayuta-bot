﻿using Discord.WebSocket;

namespace nayuta.Commands
{
    public class CommandOsuRecent : CommandOsu
    {
        public CommandOsuRecent() : base("recent", "Show most recent plays")
        {
        }

        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            return "wip";
        }
    }
}