using System.Web;

namespace nayuta.Modules.Weather
{
    public class WeatherApi
    {
        private static string apiUrl = "http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}";

        public static WeatherData GetWeatherInLocation(string location)
        {
            WeatherData result = APIHelper<WeatherData>.GetData(string.Format(apiUrl, HttpUtility.HtmlEncode(location), APIKeys.WeatherAPIKey));

            return result;
        }
    }
}