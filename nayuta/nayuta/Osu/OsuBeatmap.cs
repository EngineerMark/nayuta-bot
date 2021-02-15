using Newtonsoft.Json;

namespace nayuta.Osu
{
    public class OsuBeatmap
    {
        [JsonProperty("beatmapset_id")]
        public string BeatmapSetID { get; set; }
        
        [JsonProperty("beatmap_id")]
        public string BeatmapID { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("version")]
        public string DifficultyName { get; set; }
        
        [JsonProperty("diff_aim")]
        public string StarratingAim { get; set; }
        
        [JsonProperty("diff_speed")]
        public string StarratingSpeed { get; set; }
        
        [JsonProperty("difficultyrating")]
        public float Starrating { get; set; }
        
        [JsonProperty("max_combo")]
        public string MaxCombo { get; set; }
    }
}