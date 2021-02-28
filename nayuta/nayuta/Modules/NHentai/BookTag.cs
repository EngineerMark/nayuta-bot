using Newtonsoft.Json;

namespace nayuta.Modules.NHentai
{
    public class BookTag
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}