using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace nayuta.Commands
{
    public class CommandEcchi : Command
    {
        private readonly string danbooruApiKey = "REtAJaPh6NRaCWuxRVSQiwTJ";
        private readonly string danbooruUser = "Amayakase";

        private readonly string apiUrl;

        public CommandEcchi() : base("hentai")
        {
            returnFunc = CommandHandler;
            InputValue = true;

            apiUrl = "https://danbooru.donmai.us/posts/random.json?login=" + danbooruUser + "&api_key=" + danbooruApiKey;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            List<string> inputArray = input.Split(' ').ToList();
            inputArray = inputArray.Select(s=>
            {
                return "*_" + HttpUtility.UrlEncode(s) + "_*";
            }).ToList();


            //string test = apiUrl+"&tags=" + ("*_"+safeInput+"_*")+"%20rating:e"+"&limit=100&order=score";
            string tagUrl = "&tags="+(string.Join(" ", inputArray))+"%20rating:e";
            //return "`"+tagUrl+"`";
            DanbooruResult result = APIHelper<DanbooruResult>.GetData(apiUrl + tagUrl);

            //DanbooruResult randomResult = result[new Random().Next(0, result.Count)];

            //return "Result value: " + randomResult.FileUrl;


            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = "Heres some",
                ImageUrl = result.FileUrl,
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Character(s)",
                        Value = result.Characters
                    }  
                },
                Footer = new EmbedFooterBuilder()
                {
                    Text = "Uploaded by "+result.Uploader
                }
            };
            
            return embed;
        }
    }
}