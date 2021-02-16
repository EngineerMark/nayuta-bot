using System;
using System.Collections;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using nayuta.Coroutine;
using nayuta.Internal;

namespace nayuta
{
    public class CommandManager : Manager<CommandManager>
    {
        //public static CommandManager Instance;

        public Bot bot;
        
        private List<Command> commands = new List<Command>();

        public List<Command> Commands
        {
            get => commands;
            private set => commands = value;
        }

        public CommandManager(Bot bot)
        {
            //Instance = this;
            this.bot = bot;
        }

        public void RegisterCommand(Command command)
        {
            commands.Add(command);
            command.ParentManager = this;
        }

        public IEnumerator ProcessCommands(Bot bot, SocketMessage socketMessage)
        {
            string parsedMessage = socketMessage.Content.ToLower();
            if (socketMessage.Author.IsBot)
                yield break;

            InternalUser user = DatabaseManager.Instance.GetUser(socketMessage.Author.Id);
            if (user == null)
                user = new InternalUser() {DiscordID = socketMessage.Author.Id.ToString(),IsNewUser = true};
            
            foreach (Command command in commands)
            {
                dynamic status = command.Handle(socketMessage);
                bool success = true;
                
                if (status.GetType().Equals(typeof(string)))
                    Yielder.Instance.StartCoroutine(bot.SendStringMessage(socketMessage, (string) status));
                else if (status.GetType().Equals(typeof(EmbedBuilder)))
                    Yielder.Instance.StartCoroutine(bot.SendEmbedMessage(socketMessage, ((EmbedBuilder)status).Build()));
                else if (status.GetType().Equals(typeof(Embed)))
                    Yielder.Instance.StartCoroutine(bot.SendEmbedMessage(socketMessage, (Embed) status));
                else
                    success = false;

                if (success)
                {
                    DatabaseManager.Instance.UpdateUser(user);
                    // Yielder.Instance.StartCoroutine((bot.SendStringMessage(socketMessage, "Debug: latency is "+Math.Abs(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() -
                    //     socketMessage.CreatedAt.ToUnixTimeMilliseconds())+"ms")));
                }
            }
            
            yield return null;
        }
    }
}