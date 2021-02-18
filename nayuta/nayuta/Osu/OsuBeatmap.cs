using System;
using Newtonsoft.Json;

namespace nayuta.Osu
{
    public class OsuBeatmap
    {
        public OsuMode Mode { get; set; }
        
        public OsuMods Mods { get; set; }
        
        [JsonProperty("beatmapset_id")]
        public string BeatmapSetID { get; set; }
        
        [JsonProperty("beatmap_id")]
        public string BeatmapID { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("artist")]
        public string Artist { get; set; }
        
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
        
        [JsonProperty("diff_size")]
        public float CircleSize { get; set; }
        
        [JsonProperty("diff_overall")]
        public float OverallDifficulty { get; set; }
        
        [JsonProperty("diff_approach")]
        public float ApproachRate { get; set; }
        
        [JsonProperty("diff_drain")]
        public float Drain { get; set; }
        
        [JsonProperty("creator")]
        public string Mapper { get; set; }
        
        [JsonProperty("creator_id")]
        public string MapperID { get; set; }

        public float CS {
            get
            {
                float value = CircleSize;
                if ((Mods & OsuMods.HardRock) != 0)
                    value *= 1.3f;
                if ((Mods & OsuMods.Easy) != 0)
                    value *= 0.5f;
                return Math.Min(value, 10f);
            }
        }

        public float OD => OsuApi.CalculateDifficulty(Mods, OverallDifficulty);

        /// <summary>
        /// Returns converted AR
        /// </summary>
        public float AR
        {
            get
            {
                float value = OsuApi.CalculateDifficulty(Mods, ApproachRate);

                if (((Mods & OsuMods.DoubleTime) != 0 || (Mods & OsuMods.Nightcore) != 0))
                {
                    float ms = 0;
                    if (value > 5)
                        ms = 200 + (11 - value) * 100;
                    else
                        ms = 800 + (5 - value) + 80;

                    if (ms < 300)
                        value = 11;
                    else if (ms < 1200)
                        value = (11 - (ms - 300) / 150);
                    else
                        value = (5 - (ms - 1200) / 120);
                }

                return value;
            }
        }
        
        public float HP => OsuApi.CalculateDifficulty(Mods, Drain);
    }
}