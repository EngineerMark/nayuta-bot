using System.Collections.Generic;
using System.Web;
using Discord;
using Discord.WebSocket;
using Humanizer;
using nayuta.Internal;
using nayuta.Modules.Waifu;

namespace nayuta.Commands.Waifu
{
    public class CommandWaifuSearch : CommandWaifu
    {
        public CommandWaifuSearch() : base("", "Find a waifu by name or source")
        {
            InputValue = true;
            IsNsfw = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            if (input.Length <= 3)
                return "Please use a longer query";

            BetterList<Modules.Waifu.Waifu> waifuResults = WaifuApi.GetWaifus(input);
            //return waifuResults+"";
            if (waifuResults != null && waifuResults.Count > 0)
            {
                Modules.Waifu.Waifu waifu = waifuResults[0];

                EmbedBuilder embed;

                if (arguments.Get("img")!=null || arguments.Get("image")!=null)
                {
                    if (waifu.Images != null && waifu.Images.Count > 0)
                    {
                        WaifuImage image;
                        int tries = 0;
                        do
                        {
                            image = waifu.Images.Random();
                            tries++;
                            if (tries == 5)
                                return "Seems like theres a problem with the image URLs";
                        } while (!APIHelper<bool>.IsUrlValid("https://www.mywaifu.net/api.php?type=image&q="+image.ID));

                        //return "Test: " + "https://www.mywaifu.net/api.php?type=image&q=" + image.ID;
                        embed = new EmbedBuilder()
                        {
                            Title = "Random image for "+waifu.Name+"",
                            ImageUrl = "https://www.mywaifu.net/api.php?type=image&q="+image.ID
                        };
                    }
                    else
                    {
                        return "No images available for "+waifu.Name+".. Maybe in the future";
                    }
                }else
                    embed = (EmbedBuilder) WaifuApi.GetWaifuEmbed(waifu);
                return embed;
            }
            else
            {
                return "No waifu found from your search input!";
            }
        }
    }
}