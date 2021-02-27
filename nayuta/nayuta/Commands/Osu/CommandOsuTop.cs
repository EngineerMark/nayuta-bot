using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Humanizer;
using nayuta.Math;
using nayuta.Modules.Osu;

namespace nayuta.Commands.Osu
{
    public class CommandOsuTop : CommandOsu
    {
        public CommandOsuTop() : base("top", "Show top plays of a player")
        {
            InputValue = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            ApplyMode(arguments);
            ApplyPlayer(socketMessage.Author.Id, input);
            
            if (_osuUser == null)
                return "No user has been found.";

            bool getRecent = false;
            int limit = 3;

            if (arguments.Get("r") != null)
                getRecent = true;

            if (arguments.Get("l") != null || arguments.Get("limit") != null)
                int.TryParse((arguments.Get("l") != null) ? arguments.Get("l") : arguments.Get("limit"), out limit);

            if (limit < 3)
                limit = 3;
            if (limit > 5)
                limit = 5;

            List<OsuPlay> topPlays = OsuApi.GetUserBest(_osuUser, _osuMode, 100, false);

            if (topPlays.Count == 0)
                return "This player has no top plays in osu!" + _osuMode;

            if (topPlays.Count < limit)
                limit = topPlays.Count;

            List<KeyValuePair<int, OsuPlay>> resultPlays = null;
            if (getRecent)
                resultPlays = topPlays.Select((x, i)=>new KeyValuePair<int, OsuPlay>(i+1, x)).OrderBy(i=> ((DateTimeOffset)DateTime.Parse(i.Value.DateAchieved)).ToUnixTimeSeconds()).Reverse().Take(limit).ToList();
            else
                resultPlays = topPlays.Select((x, i)=>new KeyValuePair<int, OsuPlay>(i+1, x)).Take(limit).ToList();

            EmbedFieldBuilder topPlaysField = new EmbedFieldBuilder()
            {
                Name = "\u200B",
                Value = "a",
                IsInline = false
            };

            EmbedBuilder embed = new EmbedBuilder()
            {
                Color = ParentManager.bot.BotColor,
                Title = (getRecent ? "Most "+limit+" recent top plays" : "Top "+limit+" plays")+" from **"+_osuUser.Name+"**",
                ThumbnailUrl = "https://a.ppy.sh/" + _osuUser.ID,
                Description = "Plays on osu!"+_osuMode,
                Fields = new List<EmbedFieldBuilder>()
                {
                }
            };
            
            foreach (KeyValuePair<int, OsuPlay> playPair in resultPlays)
            {
                int playIndex = playPair.Key;
                OsuPlay play = playPair.Value;
                
                play.Beatmap = OsuApi.GetBeatmap(play.MapID, play.Mods, play.Mode);
                
                EmbedFieldBuilder field = new EmbedFieldBuilder()
                {
                    Name = "**"+playIndex+". "+play.Beatmap.Title+" ["+play.Beatmap.DifficultyName+"] +"+(((OsuModsShort) play.Mods).ModParser() + "").Replace(", ", "")+"** ("+Mathf.Round((float)play.Beatmap.Starrating, 2)+"\\*)",
                    Value =  OsuRanks.GetEmojiFromRank(play.Rank).ToString()+" "+Mathf.Round(play.Accuracy, 2)+"% • **"+Mathf.Round(play.Performance.CurrentValue, 2)+"pp** "+play.MaxCombo + "x"+(play.Beatmap.MaxCombo!=null?"/" + play.Beatmap.MaxCombo+"x":"")+" "+((play.IsFullcombo=="1"?"":"(For FC: **"+Mathf.Round(play.Performance.CurrentValueIfFC, 2)+"pp**)"))+" • "+play.Score.FormatNumber()+"\n" +
                                  "Achieved "+DateTime.Parse(play.DateAchieved).Humanize() + " • [Map](https://osu.ppy.sh/beatmaps/" + play.Beatmap.BeatmapID + ")",
                    IsInline = false
                };

                embed.AddField(field);
            }
            
            return embed;
        }
    }
}