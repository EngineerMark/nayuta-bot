using System.Collections.Generic;
using Discord;
using Discord.WebSocket;

namespace nayuta.Commands
{
    public class CommandHelp : Command
    {
        public CommandHelp() : base("help", "Displays a list of available commands and other information")
        {
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            EmbedFieldBuilder commandList = new EmbedFieldBuilder();
            commandList.Name = "Commands";

            string commandListValue = "";
            foreach (Command command in ParentManager.Commands)
            {
                if(command.DisplayInHelp)
                    commandListValue += ParentManager.bot.Prefix + command.CommandName + " - "+(string.IsNullOrEmpty(command.CommandDescription)?"No description":command.CommandDescription)+"\n";
            }
            commandList.Value = commandListValue;

            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = "Information about Nayuta",
                ThumbnailUrl = ParentManager.bot.DiscordClient.CurrentUser.GetAvatarUrl(),
                Color = ParentManager.bot.BotColor,
                Fields = new List<EmbedFieldBuilder>()
                {
                    commandList
                },
            };

            return embed;
        }
        
    }
}