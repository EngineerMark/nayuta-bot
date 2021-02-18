using System;
using Humanizer;
using Newtonsoft.Json;

namespace nayuta.Osu
{
    public class OsuPlay
    {
        public OsuMode Mode { get; set; }

        public enum ResultMode
        {
            Short,
            Expansive
        }

        [JsonProperty("beatmap_id")] public string MapID { get; set; }

        [JsonProperty("score_id")] public string ScoreID { get; set; }

        [JsonProperty("score")] public int Score { get; set; }

        [JsonProperty("rank")] public string Rank { get; set; }

        [JsonProperty("pp")] public float Performance { get; set; }

        [JsonProperty("enabled_mods")] public OsuMods Mods { get; set; }

        [JsonProperty("maxcombo")] public string MaxCombo { get; set; }

        [JsonProperty("perfect")] public string IsFullcombo { get; set; }

        [JsonProperty("date")] public string DateAchieved { get; set; }

        [JsonProperty("count50")] public float C50 { get; set; }

        [JsonProperty("count100")] public float C100 { get; set; }

        [JsonProperty("count300")] public float C300 { get; set; }

        [JsonProperty("countkatu")] public float CKatu { get; set; }

        [JsonProperty("countgeki")] public float CGeki { get; set; }

        [JsonProperty("countmiss")] public float CMiss { get; set; }

        [JsonIgnore] public float Accuracy => OsuApi.CalculateAccuracy(Mode, CMiss, C50, C100, C300, CKatu, CGeki);

        [JsonIgnore] public OsuBeatmap Beatmap { get; set; }

        public string GetInfo(ResultMode resultMode = ResultMode.Short)
        {
            string result = "";
            switch (resultMode)
            {
                case ResultMode.Short:
                    result = "[" + Beatmap.Title + " \\[" + Beatmap.DifficultyName +
                             "\\]](https://osu.ppy.sh/beatmaps/" + Beatmap.BeatmapID + ") +" +
                             ("" + ((OsuModsShort) Mods).ModParser() + "").Replace(", ", "") + "** (" +
                             Math.Round(Beatmap.Starrating, 1) + "\\*)\n" +
                             OsuRanks.GetEmojiFromRank(Rank).ToString() + " • **" +
                             Math.Round(Performance).FormatNumber() + "pp** • " + Math.Round(Accuracy, 2) + "% • " +
                             MaxCombo + "/" + Beatmap.MaxCombo + (IsFullcombo == "1" ? " FC" : "") + " • " +
                             Score.FormatNumber() + "\n" +
                             "Achieved " + DateTime.Parse(DateAchieved).Humanize();
                    break;
                case ResultMode.Expansive:
                    break;
            }

            return result;
        }
    }
}