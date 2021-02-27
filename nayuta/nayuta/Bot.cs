using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using nayuta.Commands;
using nayuta.Coroutine;
using nayuta.Internal;
using nayuta.Modules.Waifu;

namespace nayuta
{
    public class Bot
    {
        private CommandManager _commandManager;
        private GuildThreadManager _guildThreadManager;
        private DatabaseManager _databaseManager;
        private InternalUserManager _internalUserManager;
        
        private DiscordSocketClient _discordClient;
        private readonly string _discordToken;
        private readonly Color _botColor = new Color(255, 153, 153);
        
        public string Prefix { get; }

        public DiscordSocketClient DiscordClient
        {
            get => _discordClient;
        }
        
        public Color BotColor
        {
            get => _botColor;
        }
        
        public Bot(string token, string prefix)
        {
            //Test
            this.Prefix = prefix;
            _discordToken = token;

            _commandManager = new CommandManager(this);
            _commandManager.RegisterCommand(new CommandHelp());
            _commandManager.RegisterCommand(new CommandPing());
            _commandManager.RegisterCommand(new CommandEcchi());
            _commandManager.RegisterCommand(new CommandSystem());
            _commandManager.RegisterCommand(new CommandWeather());
            _commandManager.RegisterCommand(new CommandOsuRegister());
            _commandManager.RegisterCommand(new CommandOsuProfile());
            _commandManager.RegisterCommand(new CommandOsuRecent());
            _commandManager.RegisterCommand(new CommandOsuTop());
            _commandManager.RegisterCommand(new CommandWaifuSearch());

            _databaseManager = new DatabaseManager();
            _internalUserManager = new InternalUserManager();
            _guildThreadManager = new GuildThreadManager();
            
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
            //Yielder.Instance.StartCoroutine(_commandManager.ProcessCommands(this, socketMessage));
            _guildThreadManager.EnqueueCommand(this, socketMessage);
        }

        public IEnumerator SendStringMessage(SocketMessage sourceMessage, string message)
        {
            //yield return WaitFor.WaitForSeconds(5);
            sourceMessage.Channel.SendMessageAsync(message);
            yield return null;
        }

        public IEnumerator SendEmbedMessage(SocketMessage sourceMessage, Embed embed)
        {
            sourceMessage.Channel.SendMessageAsync(null, false, embed);
            yield return null;
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
