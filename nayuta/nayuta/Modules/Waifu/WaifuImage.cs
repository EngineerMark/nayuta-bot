using System;
using Newtonsoft.Json;

namespace nayuta.Modules.Waifu
{
    [Serializable]
    public struct WaifuImage
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("imageurl")]
        public string Url { get; set; }
    }
}