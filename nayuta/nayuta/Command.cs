using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace nayuta
{
    public abstract class Command : ICommand
    {
        /// <summary>
        /// If true, the command will test any string text after the command itself
        /// </summary>
        public bool InputValue { get; set; }
        private string commandName;
        private string commandDescription;
        private bool displayInHelp;

        public CommandManager ParentManager { get; set; }

        public delegate object ReturnFunc(SocketMessage socketMessage, string input, CommandArguments arguments);

        protected ReturnFunc returnFunc;
        
        public string CommandName
        {
            get => commandName;
            private set => commandName = value;
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
            this.commandName = commandName;
            this.returnFunc = CommandHandler;
            this.commandDescription = commandDescription;
            this.displayInHelp = displayInHelp;
        }

        public abstract object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments);

        public dynamic Handle(SocketMessage socketMessage, string userInput, CommandArguments arguments)
        {
            string inputString = userInput.ToLower();
            string enteredCommand = inputString;
            List<string> splitInputString = enteredCommand.Split(' ').ToList();
            if (enteredCommand.Contains(" "))
                enteredCommand = splitInputString[0];
            if(splitInputString.Count>0)
                splitInputString.RemoveAt(0);

            string inputStringAdditional = splitInputString.Count>0?splitInputString.Aggregate((i, j) => i + " " + j):"";

            //return "test value: " + arguments.ToString();

            if (enteredCommand == CommandManager.Instance.bot.Prefix + commandName)
                return returnFunc(socketMessage, InputValue?inputStringAdditional:null, arguments);
            
            return false;
        }
    }
}