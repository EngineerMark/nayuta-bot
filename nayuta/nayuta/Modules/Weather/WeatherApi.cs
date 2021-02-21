using System.Web;

namespace nayuta.Modules.Weather
{
    public class WeatherApi
    {
        private static string apiUrl = "http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}";

        public enum TemperatureType
        {
            Kelvin,
            Celsius,
            Fahrenheit
        }

        public static WeatherData GetWeatherInLocation(string location)
        {
            WeatherData result = APIHelper<WeatherData>.GetData(string.Format(apiUrl, HttpUtility.HtmlEncode(location), APIKeys.WeatherAPIKey));

            return result;
        }

        public static float ConvertTemperature(float value, TemperatureType from, TemperatureType to)
        {
            switch (from)
            {
                case TemperatureType.Kelvin:
                    switch (to)
                    {
                        case TemperatureType.Kelvin:
                            break;
                        case TemperatureType.Celsius:
                            value = value - 273.15f;
                            break;
                        case  TemperatureType.Fahrenheit:
                            value = (value - 273.15f) * 9f / 5f + 32f;
                            break;
                    }
                    break;
                case TemperatureType.Celsius:
                    switch (to)
                    {
                        case TemperatureType.Kelvin:
                            value = value + 273.15f;
                            break;
                        case TemperatureType.Celsius:
                            break;
                        case  TemperatureType.Fahrenheit:
                            value = value * 9f / 5f + 32;
                            break;
                    }
                    break;
                case TemperatureType.Fahrenheit:
                    switch (to)
                    {
                        case TemperatureType.Kelvin:
                            value = (value - 32f) * 5f / 9f + 273.15f;
                            break;
                        case TemperatureType.Celsius:
                            value = (value - 32f) * 5f / 9f;
                            break;
                        case  TemperatureType.Fahrenheit:
                            break;
                    }
                    break;
            }
            
            return value;
        }
    }
}