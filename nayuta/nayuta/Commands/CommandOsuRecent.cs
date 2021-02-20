using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Humanizer;
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
                Title = "Most recent play by **"+_osuUser.Name+"** in osu!"+_osuMode,
                Description = "["+recentPlay.Beatmap.Artist+" - "+recentPlay.Beatmap.Title+" \\["+recentPlay.Beatmap.DifficultyName+"\\]](https://osu.ppy.sh/beatmaps/"+recentPlay.Beatmap.BeatmapID+")\n" +
                              "Map status: "+ OsuApi.BeatmapStatusEmotes[recentPlay.Beatmap.Status].ToString() + " " + recentPlay.Beatmap.Status.ToString(),
                ThumbnailUrl = "https://b.ppy.sh/thumb/"+recentPlay.Beatmap.BeatmapSetID+"l.jpg",
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Beatmap Information",
                        Value = "("+Mathf.Round(recentPlay.Beatmap.Starrating??0, 2)+"*) CS"+Mathf.Round(recentPlay.Beatmap.MapStats.CS, 2) + " • AR"+Mathf.Round(recentPlay.Beatmap.MapStats.AR, 2)+" • OD"+Mathf.Round(recentPlay.Beatmap.MapStats.OD, 2)+" • HP"+Mathf.Round(recentPlay.Beatmap.MapStats.HP, 2)+"\n" +
                                "Mapped by ["+recentPlay.Beatmap.Mapper+"](https://osu.ppy.sh/users/"+recentPlay.Beatmap.MapperID+")",
                        IsInline = false
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Play Information",
                        Value = OsuRanks.GetEmojiFromRank(recentPlay.Rank).ToString()+" "+Mathf.Round(recentPlay.Accuracy, 2)+"% **+"+(""+((OsuModsShort) recentPlay.Mods).ModParser()+"").Replace(", ", "")+"** • **"+Mathf.Round(recentPlay.Performance.CurrentValue, 2)+"pp** "+recentPlay.MaxCombo + "x/" + recentPlay.Beatmap.MaxCombo+"x "+((recentPlay.IsFullcombo=="1"?"":"(For FC: **"+Mathf.Round(recentPlay.Performance.CurrentValueIfFC, 2)+"pp**)"))+"\n" +
                                "["+recentPlay.C300+"/"+recentPlay.C100+"/"+recentPlay.C50+"/"+recentPlay.CMiss+"] • "+recentPlay.Score.FormatNumber(),
                        IsInline = false
                    }
                },
                Footer = new EmbedFooterBuilder()
                {
                    Text = "Played "+DateTime.Parse(recentPlay.DateAchieved).Humanize(),
                }
            };
            return embed;
        }
    }
}