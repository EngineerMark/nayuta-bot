using System;
using Newtonsoft.Json;

namespace nayuta.Modules.Waifu
{
    [Serializable]
    public class WaifuResult<T>
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")] public string Message { get; set; } = "None";
        
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}