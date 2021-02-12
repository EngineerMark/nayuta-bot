using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace nayuta
{
    public class Bot
    {
        private DiscordSocketClient _discordClient;
        private readonly string _discordToken;
        private readonly string prefix;
        
        public Bot(string token, string prefix)
        {
            this.prefix = prefix;
            _discordToken = token;
            MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            _discordClient = new DiscordSocketClient();
            _discordClient.Log += Log;
            _discordClient.MessageReceived += MessageReceived;
            var token = _discordToken;

            await _discordClient.LoginAsync(TokenType.Bot, token);
            await _discordClient.StartAsync();
            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage socketMessage)
        {
            string parsedMessage = socketMessage.Content.ToLower();
            if (socketMessage.Author.IsBot)
                return;

            if (parsedMessage.Equals(prefix + "ping"))
                await SendStringMessage(socketMessage, "Pong");
        }

        private async Task SendStringMessage(SocketMessage sourceMessage, string message)
        {
            await sourceMessage.Channel.SendMessageAsync(message);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
