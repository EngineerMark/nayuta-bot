using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace nayuta
{
    public class Command
    {
        /// <summary>
        /// If true, the command will test any string text after the command itself
        /// </summary>
        public bool InputValue { get; set; }

        private string commandName;

        public delegate object ReturnFunc(SocketMessage socketMessage, string input);

        protected ReturnFunc returnFunc;
        
        public Command(string commandName, ReturnFunc returnFunc)
        {
            this.commandName = commandName;
            this.returnFunc = returnFunc;
        }

        public dynamic Handle(SocketMessage socketMessage)
        {
            string inputString = socketMessage.Content.ToLower();
            string enteredCommand = inputString;
            List<string> splitInputString = enteredCommand.Split(' ').ToList();
            if (enteredCommand.Contains(" "))
            {
                enteredCommand = splitInputString[0];
                splitInputString.RemoveAt(0);
            }

            string inputStringAdditional = splitInputString.Aggregate((i, j) => i + " " + j);

            if (enteredCommand == CommandManager.Instance.bot.Prefix + commandName)
                return returnFunc(socketMessage, InputValue?inputStringAdditional:null);
            
            return false;
        }
    }
}