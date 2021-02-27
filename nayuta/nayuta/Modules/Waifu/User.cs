using System;
using Newtonsoft.Json;

namespace nayuta.Modules.Waifu
{
    [Serializable]
    public class User
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        
        [JsonProperty("username")]
        public string Username { get; set; }

        public string Thumbnail => "https://www.mywaifu.net/uploads/ava/"+ID;
    }
}