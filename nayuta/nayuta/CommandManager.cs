using System;
using System.Collections;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using nayuta.Coroutine;

namespace nayuta
{
    public class CommandManager
    {
        public static CommandManager Instance;

        public Bot bot;
        
        private List<Command> commands = new List<Command>();

        public CommandManager(Bot bot)
        {
            Instance = this;
            this.bot = bot;
        }

        public void RegisterCommand(Command command)
        {
            commands.Add(command);
        }

        public IEnumerator ProcessCommands(Bot bot, SocketMessage socketMessage)
        {
            string parsedMessage = socketMessage.Content.ToLower();
            if (socketMessage.Author.IsBot)
                yield break;

            // if (parsedMessage.Equals(bot.Prefix + "ping"))
            //     Yielder.Instance.StartCoroutine(bot.SendStringMessage(socketMessage, "Pong"));
            foreach (Command command in commands)
            {
                var status = command.Handle(socketMessage);
                bool success = false;
                
                if (status.GetType().Equals(typeof(string)))
                {
                    Yielder.Instance.StartCoroutine(bot.SendStringMessage(socketMessage, (string) status));
                    success = true;
                }else if (status.GetType().Equals(typeof(EmbedBuilder)))
                {
                    Yielder.Instance.StartCoroutine(bot.SendEmbedMessage(socketMessage, ((EmbedBuilder)status).Build()));
                    success = true;
                }

                if (success)
                {
                    Yielder.Instance.StartCoroutine((bot.SendStringMessage(socketMessage, "Debug: latency is "+Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() -
                        socketMessage.CreatedAt.ToUnixTimeMilliseconds())+"ms")));
                }
            }
            
            yield return null;
        }
    }
}