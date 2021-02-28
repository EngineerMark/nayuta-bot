using System;
using Newtonsoft.Json;

namespace nayuta.Modules.NHentai
{
    [Serializable]
    public class BookTitle
    {
        [JsonProperty("english")]
        public string English { get; set; }
        
        [JsonProperty("japanese")]
        public string Japanese { get; set; }
        
        [JsonProperty("pretty")]
        public string Pretty { get; set; }
    }
}