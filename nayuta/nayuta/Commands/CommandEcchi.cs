using System;
using System.Collections.Generic;
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

            apiUrl = "https://danbooru.donmai.us/posts.json?login=" + danbooruUser + "&api_key=" + danbooruApiKey;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            string test = apiUrl + "&tags=" + HttpUtility.UrlEncode(input+" rating:explicit")+"&limit=100";
            WebClient client = new WebClient();
            string s = client.DownloadString(test);

            List<DanbooruResult> result = JsonConvert.DeserializeObject<List<DanbooruResult>>(s);

            DanbooruResult randomResult = result[new Random().Next(0, result.Count)];

            //return "Result value: " + randomResult.FileUrl;


            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = "Heres some",
                ImageUrl = randomResult.FileUrl,
                Footer = new EmbedFooterBuilder()
                {
                    Text = "Uploaded by "+randomResult.Uploader
                }
            };
            
            return embed;
        }
    }
}