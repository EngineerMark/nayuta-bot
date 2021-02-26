using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
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

            List<Waifu> waifuResults = WaifuApi.SearchWaifus(input);
            if (waifuResults != null && waifuResults.Count > 0)
            {
                Waifu waifu = waifuResults[0];
                string noValue = "\uD83D\uDEC7";

                EmbedBuilder embed = new EmbedBuilder()
                {
                    Title = waifu.Name,
                    Description = waifu.SourceName,
                    ImageUrl = "https://www.mywaifu.net/api.php?type=thumbnail&q="+waifu.ID,
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Bust",
                            Value = waifu.Measurements.Bust==0?noValue:waifu.Measurements.Bust+"cm",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Waist",
                            Value = waifu.Measurements.Waist==0?noValue:waifu.Measurements.Waist+"cm",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Hip",
                            Value = waifu.Measurements.Hip==0?noValue:waifu.Measurements.Hip+"cm",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Age",
                            Value = waifu.Age==0?noValue:waifu.Age,
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Height",
                            Value = waifu.Height==0?noValue:waifu.Height+"cm",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Weight",
                            Value = waifu.Weight==0?noValue:waifu.Weight+"kg",
                            IsInline = true
                        }
                    }
                };

                return embed;
            }
            else
            {
                return "No waifu found from your search input!";
            }
        }
    }
}