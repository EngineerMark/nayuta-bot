using Discord.WebSocket;
using nayuta.Internal;
using nayuta.Modules.Osu;

namespace nayuta.Commands
{
    public class CommandOsuRegister : CommandOsu
    {
        public CommandOsuRegister() : base("register", "Connects your osu account to your profile")
        {
            InputValue = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            if (string.IsNullOrEmpty(input) || input.Length<=3)
                return "Please enter a username to connect to";

            OsuUser osuUser = OsuApi.GetUser(input);
            if (osuUser == null)
                return "Unable to find any user with this username";

            InternalUser internalUser = DatabaseManager.Instance.GetUser(socketMessage.Author.Id);
            if (string.IsNullOrEmpty(internalUser.OsuUserID))
            {
                internalUser.OsuUserID = osuUser.ID.ToString();
                return "Registered '"+osuUser.Name+"' to your profile, "+socketMessage.Author.Mention;
            }
            else
            {
                internalUser.OsuUserID = osuUser.ID.ToString();
                return "Changed connection to '"+osuUser.Name+"' on your profile, "+socketMessage.Author.Mention;
            }
            
        }
    }
}