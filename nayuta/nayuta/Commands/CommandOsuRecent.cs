using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using nayuta.Math;
using nayuta.Osu;

namespace nayuta.Commands
{
    public class CommandOsuRecent : CommandOsu
    {
        public CommandOsuRecent() : base("recent", "Show latest play")
        {
            InputValue = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            input = ApplyMode(input);
            ApplyPlayer(socketMessage.Author.Id, input);
            
            if (_osuUser == null)
                return "No user has been found.";

            List<OsuPlay> recentPlays = OsuApi.GetUserRecent(_osuUser.Name, _osuMode, 1, true);
            if (recentPlays == null)
                return ""+_osuUser.Name+" has not played in the last 24 hours";

            OsuPlay recentPlay = recentPlays[0];
            
            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = "Most recent play by "+_osuUser.Name,
                Description = "["+recentPlay.Beatmap.Artist+" - "+recentPlay.Beatmap.Title+" \\["+recentPlay.Beatmap.DifficultyName+"\\] **+"+(""+((OsuModsShort) recentPlay.Mods).ModParser()+"").Replace(", ", "")+"**](https://osu.ppy.sh/beatmaps/"+recentPlay.Beatmap.BeatmapID+")",
                ThumbnailUrl = "https://b.ppy.sh/thumb/"+recentPlay.Beatmap.BeatmapSetID+"l.jpg",
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Beatmap Information",
                        Value = "("+Mathf.Round(recentPlay.Beatmap.Starrating??0, 2)+"*) CS"+recentPlay.Beatmap.MapStats.CS + " • AR"+Mathf.Round(recentPlay.Beatmap.MapStats.AR, 2)+" • OD"+recentPlay.Beatmap.MapStats.OD+" • HP"+recentPlay.Beatmap.MapStats.HP+"\n" +
                                "Mapped by ["+recentPlay.Beatmap.Mapper+"](https://osu.ppy.sh/users/"+recentPlay.Beatmap.MapperID+")",
                        IsInline = false
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Play Information",
                        Value = OsuRanks.GetEmojiFromRank(recentPlay.Rank).ToString()+" "+Mathf.Round(recentPlay.Accuracy, 2)+"% • "+Mathf.Round(recentPlay.Performance.CurrentValue, 2)+"pp "+recentPlay.MaxCombo + "/" + recentPlay.Beatmap.MaxCombo+" "+((recentPlay.IsFullcombo=="1"?"":"(For FC: "+Mathf.Round(recentPlay.Performance.CurrentValueIfFC, 2)+"pp)")),
                        IsInline = false
                    }
                }
            };
            return embed;
        }
    }
}