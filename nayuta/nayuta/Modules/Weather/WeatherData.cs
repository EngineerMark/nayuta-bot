using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace nayuta.Modules.Weather
{
    public class WeatherData
    {
        [JsonProperty("cod")]
        public int CallbackCode { get; set; }
        
        [JsonProperty("message")]
        public string ErrorMessage { get; set; }
        
        [JsonProperty("coord")]
        public WeatherCoord Coord { get; set; }
        
        [JsonProperty("base")]
        public string Base { get; set; }
        
        [JsonProperty("weather")]
        public List<WeatherInfo> Info { get; set; }
        
        [JsonProperty("main")]
        public WeatherAir Air { get; set; }
        
        [JsonProperty("visibility")]
        public float ViewDistance { get; set; }
        
        [JsonProperty("wind")]
        public WeatherWind Wind { get; set; }
        
        [JsonProperty("clouds")]
        public WeatherClouds Clouds { get; set; }
        
        [JsonProperty("dt")]
        public long CurrentTime { get; set; }
        
        [JsonProperty("name")]
        public string Location { get; set; }
        
        [JsonProperty("sys")]
        public WeatherLocation LocationInfo { get; set; }
    }

    [Serializable]
    public struct WeatherLocation
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("country")]
        public string Country { get; set; }
    }

    [Serializable]
    public struct WeatherCoord
    {
        [JsonProperty("lon")]
        public float Longitude { get; set; }
        [JsonProperty("lat")]
        public float Latitude { get; set; }
    }
    
    [Serializable]
    public struct WeatherWind
    {
        [JsonProperty("speed")]
        public float Longitude { get; set; }
        [JsonProperty("deg")]
        public float Latitude { get; set; }
    }
    
    [Serializable]
    public struct WeatherClouds
    {
        [JsonProperty("akk")]
        public float Longitude { get; set; }
    }
    
    [Serializable]
    public struct WeatherInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("main")]
        public string Main { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    [Serializable]
    public struct WeatherAir
    {
        [JsonProperty("temp")]
        public float TemperatureKelvin { get; set; }
        
        [JsonProperty("feels_like")]
        public float FeelTemperatureKelvin { get; set; }
        
        [JsonProperty("temp_min")]
        public float TemperatureLowKelvin { get; set; }
        
        [JsonProperty("temp_max")]
        public float TemperatureHighKelvin { get; set; }
        
        [JsonProperty("pressure")]
        public float AirPressure { get; set; }
        
        [JsonProperty("humidity")]
        public float AirHumidity { get; set; }

        [JsonIgnore] 
        public float TemperatureCelcius => TemperatureKelvin - 273.15f;
        
        [JsonIgnore]
        public float FeelTemperatureCelcius => FeelTemperatureKelvin - 273.15f;
        
        [JsonIgnore]
        public float TemperatureLowCelcius => TemperatureLowKelvin - 273.15f;
        
        [JsonIgnore]
        public float TemperatureHighCelcius => TemperatureHighKelvin - 273.15f;
    }
}