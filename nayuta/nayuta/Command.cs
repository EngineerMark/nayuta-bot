using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using nayuta.Internal;

namespace nayuta
{
    public abstract class Command : ICommand
    {
        /// <summary>
        /// If true, the command will test any string text after the command itself
        /// </summary>
        public bool InputValue { get; set; }
        public bool IsNsfw { get; set; } = false;
        private string commandName; // Main command name, used for references etc
        private string commandDescription;
        private bool displayInHelp;

        protected BetterList<string> CommandNames { get; set; } = new BetterList<string>();

        public CommandManager ParentManager { get; set; }

        public delegate object ReturnFunc(SocketMessage socketMessage, string input, CommandArguments arguments);

        protected ReturnFunc returnFunc;

        protected void RegisterCommandName(string commandName)
        {
            if(!CommandNames.Contains(commandName.ToLower()))
                CommandNames.Add(commandName.ToLower());
        }
        
        public string CommandName
        {
            get => commandName;
            private set
            {
                if(!CommandNames.Contains(value.ToLower()))
                    CommandNames.Add(value.ToLower());
                commandName = value;
            }
        }
        public string CommandDescription
        {
            get => commandDescription;
            private set => commandDescription = value;
        }
        
        public bool DisplayInHelp
        {
            get => displayInHelp;
            private set => displayInHelp = value;
        }
        
        public Command(string commandName, string commandDescription = null, bool displayInHelp = true)
        {
            this.CommandName = commandName;
            this.returnFunc = CommandHandler;
            this.commandDescription = commandDescription;
            this.displayInHelp = displayInHelp;
        }

        public abstract object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments);

        public dynamic Handle(SocketMessage socketMessage, string userInput, CommandArguments arguments)
        {
            if (!((SocketTextChannel) socketMessage.Channel).IsNsfw && IsNsfw)
                return "This command is only allowed in NSFW marked channels";
            
            string inputString = userInput.ToLower();
            string enteredCommand = inputString;
            List<string> splitInputString = enteredCommand.Split(' ').ToList();
            if (enteredCommand.Contains(" "))
                enteredCommand = splitInputString[0];
            if(splitInputString.Count>0)
                splitInputString.RemoveAt(0);

            string inputStringAdditional = splitInputString.Count>0?splitInputString.Aggregate((i, j) => i + " " + j):"";

            //return "test value: " + arguments.ToString();

            //if (enteredCommand == CommandManager.Instance.bot.Prefix + commandName)
            if (CommandNames.FindAll(a => String.Equals((CommandManager.Instance.bot.Prefix + a), enteredCommand, StringComparison.CurrentCultureIgnoreCase)).Count>0)
                return returnFunc(socketMessage, InputValue?inputStringAdditional:null, arguments);
            
            return false;
        }
    }
}