using System;
using System.Collections.Generic;
using System.Web;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Humanizer.Localisation;
using nayuta.Internal;
using nayuta.Math;
using nayuta.Modules.Osu;

namespace nayuta.Commands.Osu
{
    public class CommandOsuProfile : CommandOsu
    {
        public CommandOsuProfile() : base("profile", "View user profile (*use -m to view a specific gamemode*)")
        {
            InputValue = true;
        }

        public override object CommandHandler(SocketMessage socketMessage, string input, CommandArguments arguments)
        {
            ApplyMode(arguments);
            ApplyPlayer(socketMessage.Author.Id, input);

            //List<OsuUser> users = APIHelper<List<OsuUser>>.GetData(apiUrl + "get_user?u&k="+apiKey+"&u=" + username);
            if (_osuUser == null)
                return "No user has been found.";
            else
            {
                EmbedFieldBuilder topplayField = new EmbedFieldBuilder
                {
                    Name = "Top plays",
                    Value = "No top plays available",
                    IsInline = false
                };

                List<OsuPlay> topPlays = OsuApi.GetUserBest(_osuUser, _osuMode, 3, true);
                if (topPlays != null && topPlays.Count > 0)
                {
                    string topPlayString = "";
                    int i = 0;
                    topPlays.ForEach(play =>
                    {
                        i++;
                        string stringified = "**" + i + ". " + "[" + play.Beatmap.Title + " \\[" + play.Beatmap.DifficultyName +
                                             "\\]](https://osu.ppy.sh/beatmaps/" + play.Beatmap.BeatmapID + ") +" +
                                             ("" + ((OsuModsShort) play.Mods).ModParser() + "").Replace(", ", "") + "** (" +
                                             Mathf.Round(play.Beatmap.Starrating??0, 1) + "\\*)\n" +
                                             OsuRanks.GetEmojiFromRank(play.Rank).ToString() + " • **" +
                                             Mathf.Round(play.Performance.CurrentValue).FormatNumber() + "pp** • " + Mathf.Round(play.Accuracy, 2) + "% • " +
                                             play.MaxCombo + (play.Beatmap.MaxCombo!=null?"/"+play.Beatmap.MaxCombo:"x")  + (play.IsFullcombo == "1" ? " FC" : "") + " • " +
                                             play.Score.FormatNumber() + "\n" +
                                             "Achieved " + DateTime.Parse(play.DateAchieved).Humanize();
                        topPlayString += stringified + "\n";
                    });
                    topplayField.Value = topPlayString;
                }

                EmbedBuilder embed = new EmbedBuilder()
                {
                    Color = ParentManager.bot.BotColor,
                    Title = "Profile of " + _osuUser.Name,
                    ThumbnailUrl = "https://a.ppy.sh/" + _osuUser.ID,
                    Description = "Showing osu!" + _osuMode + " statistics",
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Rank",
                            Value =
                                _osuUser.Performance == 0
                                    ? "No PP or inactive"
                                    : ("#" + _osuUser.Globalrank.FormatNumber() + " (:flag_" + _osuUser.CountryCode.ToLower() +
                                       ": #" + _osuUser.Countryrank.FormatNumber() + ")"),
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Performance",
                            Value = Mathf.Round(_osuUser.Performance).FormatNumber() + "pp",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Level",
                            Value = Mathf.Floor(_osuUser.Level) + " (" +
                                    Mathf.Round((Mathf.Round(_osuUser.Level, 2) - Mathf.Floor(_osuUser.Level)) * 100) +
                                    "% to level " + (Mathf.Ceil(_osuUser.Level)) + ")",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Ranked Score",
                            Value = _osuUser.RankedScore.FormatNumber(),
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Playtime",
                            Value = TimeSpan.FromSeconds(_osuUser.Playtime).Humanize(maxUnit: TimeUnit.Hour),
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Maps played",
                            Value = _osuUser.Playcount.FormatNumber(),
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Total Score",
                            Value = _osuUser.TotalScore.FormatNumber(),
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Ranks",
                            Value = OsuRanks.GetEmojiFromRank("X")+" "+_osuUser.GetCountRankSS().FormatNumber()+" " +
                                    "• "+OsuRanks.GetEmojiFromRank("S")+" "+_osuUser.GetCountRankS().FormatNumber()+" " +
                                    "• "+OsuRanks.GetEmojiFromRank("A")+" "+_osuUser.GetCountRankA().FormatNumber(),
                            IsInline = false
                        },
                        topplayField
                    },
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "User registered " + DateTime.Parse(_osuUser.Joindate).Humanize()
                    }
                };

                return embed;
            }
        }
    }
}