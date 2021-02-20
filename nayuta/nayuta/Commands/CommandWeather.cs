using System.Collections.Generic;
using System.Globalization;
using Discord;
using Discord.WebSocket;
using Humanizer;
using nayuta.Math;
using nayuta.Modules.Weather;

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
            if (input.Length == 0)
                return "Please enter a location to check weather.";

            WeatherData data = WeatherApi.GetWeatherInLocation(input);

            if (data.CallbackCode != 200)
                return "Error fetching data from API, please try again later";

            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = ParentManager.bot.BotColor,
                Title = "Current weather for "+ (new CultureInfo("en-US", false).TextInfo.ToTitleCase(data.Location))+" :flag_" + data.LocationInfo.Country.ToLower()+":",
                ThumbnailUrl = string.Format("http://openweathermap.org/img/wn/{0}@2x.png", data.Info[0].Icon),
                Description = data.Info[0].Main,
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Temperature",
                        Value = ""+Mathf.Round(data.Air.TemperatureCelcius, 1)+"°C",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Maximum Temperature",
                        Value = ""+Mathf.Round(data.Air.TemperatureHighCelcius, 1)+"°C",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Minimum Temperature",
                        Value = ""+Mathf.Round(data.Air.TemperatureLowCelcius, 1)+"°C",
                        IsInline = true
                    }
                }
            };

            return embed;
        }
    }
}