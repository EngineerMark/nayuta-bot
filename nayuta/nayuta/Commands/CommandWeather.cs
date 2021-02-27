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
                Description = "Condition: "+data.Info[0].Main,
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Temperature",
                        Value = ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.TemperatureKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Celsius), 1)+"°C / " +
                                ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.TemperatureKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Fahrenheit), 1)+"°F" +
                                "\nFeels like "+Mathf.Round(Mathf.ConvertTemperature(data.Air.FeelTemperatureKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Celsius), 1)+"°C / " +
                                ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.FeelTemperatureKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Fahrenheit), 1)+"°F",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "High",
                        Value = ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.TemperatureHighKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Celsius), 1)+"°C / " +
                                ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.TemperatureHighKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Fahrenheit), 1)+"°F",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Low",
                        Value = ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.TemperatureLowKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Celsius), 1)+"°C / " +
                                ""+Mathf.Round(Mathf.ConvertTemperature(data.Air.TemperatureLowKelvin, Mathf.TemperatureType.Kelvin, Mathf.TemperatureType.Fahrenheit), 1)+"°F",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Air Pressure",
                        Value = ""+data.Air.AirPressure+"hPa",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Humidity",
                        Value = ""+Mathf.Round(data.Air.AirHumidity, 1)+"%",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Visibility",
                        Value = (data.ViewDistance>=1000?Mathf.Round(data.ViewDistance/1000f, 0)+"km":data.ViewDistance+"m"),
                        IsInline = true
                    }
                }
            };

            return embed;
        }
    }
}