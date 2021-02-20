using Discord.WebSocket;

namespace nayuta.Commands
{
    public class CommandWeather : Command
    {
        public CommandWeather() : base("weather", "Fetch the weather for given area")
        {
            InputValue = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            return "Not implemented yet.";
        }
    }
}