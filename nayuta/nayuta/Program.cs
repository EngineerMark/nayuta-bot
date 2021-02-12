using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nayuta
{
    class Program
    {
        private DiscordSocketClient _client;

        public static void Main(string[] args)
        {
            new Bot("ODA5OTEyMTUzNTgyNzMxMzI0.YCb_eA.E6W2dgDkUrcO1ptZvlnPMX2yo3w", "n!");
        }
    }
}
