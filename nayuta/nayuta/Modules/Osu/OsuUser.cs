using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace nayuta.Modules.Osu
{
    public struct Userpage
    {
        public PlayHistory PlayHistory;
    }

    public struct PlayHistory
    {
        public PlayHistoryNode[] HistoryNodes;
    }

    public struct PlayHistoryNode
    {
        public string Month;
        public string Playtime;
    }
    
    public class OsuUser
    {
        [JsonProperty("user_id")]
        public int ID { get; set; }
        
        [JsonProperty("username")]
        public string Name { get; set; }
        
        [JsonProperty("pp_rank")]
        public int Globalrank { get; set; }
        
        [JsonProperty("pp_country_rank")]
        public int Countryrank { get; set; }
        
        [JsonProperty("country")]
        public string CountryCode { get; set; }
        
        [JsonProperty("pp_raw")]
        public float Performance { get; set; }
        
        [JsonProperty("level")]
        public float Level { get; set; }
        
        [JsonProperty("ranked_score")]
        public string RankedScore { get; set; }
        
        [JsonProperty("total_score")]
        public string TotalScore { get; set; }
        
        [JsonProperty("total_seconds_played")]
        public int Playtime { get; set; }
        
        [JsonProperty("join_date")]
        public string Joindate { get; set; }
        
        [JsonProperty("playcount")]
        public int Playcount { get; set; }

        [JsonProperty("count_rank_ss")] 
        public int CountRankSS { get; set; }
        
        [JsonProperty("count_rank_ssh")] 
        public int CountRankSSH { get; set; }
        
        [JsonProperty("count_rank_s")] 
        public int CountRankS { get; set; }
        
        [JsonProperty("count_rank_sh")] 
        public int CountRankSH { get; set; }
        
        [JsonProperty("count_rank_a")] 
        public int CountRankA { get; set; }

        public int GetCountRankSS() => CountRankSS + CountRankSSH;
        public int GetCountRankS() => CountRankS + CountRankSH;
        public int GetCountRankA() => CountRankA;


        public Userpage Userpage { get; set; }

        public void SetUserpage()
        {
            string url = "https://osu.ppy.sh/users/"+ID;
            string data = APIHelper<string>.GetDataFromWeb(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(data);
            
            var htmlBody = doc.DocumentNode.SelectSingleNode("//body");
            
            //Playhistory
            HtmlNodeCollection graphNodes = doc.DocumentNode.SelectNodes("//g[@class='line-chart__axis line-chart__axis--x']");
            
            foreach (HtmlNode node in graphNodes)
            {
                string value = node.InnerText;
                // etc...
            }
        }
    }
}