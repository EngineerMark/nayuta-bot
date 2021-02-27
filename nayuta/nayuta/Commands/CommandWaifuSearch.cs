using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Humanizer;
using nayuta.Internal;
using nayuta.Modules.Waifu;

namespace nayuta.Commands
{
    public class CommandWaifuSearch : CommandWaifu
    {
        public CommandWaifuSearch() : base("", "Find a waifu by name or source")
        {
            InputValue = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            if (input.Length <= 3)
                return "Please use a longer query";

            BetterList<Waifu> waifuResults = WaifuApi.GetWaifus(input);
            //return waifuResults+"";
            if (waifuResults != null && waifuResults.Count > 0)
            {
                Waifu waifu = waifuResults[0];

                EmbedBuilder embed = (EmbedBuilder) WaifuApi.GetWaifuEmbed(waifu);
                return embed;
            }
            else
            {
                return "No waifu found from your search input!";
            }
        }
    }
}