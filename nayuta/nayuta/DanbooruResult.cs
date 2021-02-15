using Newtonsoft.Json;

namespace nayuta
{
    public struct DanbooruResult
    {
        [JsonProperty("ID")]
        public int ID { get; set; }
        
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        
        [JsonProperty("md5")]
        public string MD5 { get; set; }
        
        [JsonProperty("file_url")]
        public string FileUrl { get; set; }
        
        [JsonProperty("tag_string_artist")]
        public string Uploader { get; set; }
        
        [JsonProperty("tag_string_character")]
        public string Characters { get; set; }
    }
}