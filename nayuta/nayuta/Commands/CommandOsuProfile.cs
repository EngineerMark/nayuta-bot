using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Discord;
using Discord.WebSocket;
using Humanizer;
using Humanizer.Localisation;
using nayuta.Osu;

namespace nayuta.Commands
{
    public class CommandOsuProfile : CommandOsu
    {
        public CommandOsuProfile() : base("profile", "View user profile")
        {
            InputValue = true;
        }
        
        public override object CommandHandler(SocketMessage socketMessage, string input)
        {
            if (string.IsNullOrEmpty(input))
                return "Please enter a username to view their profile";
            OsuMode mode = OsuMode.Standard;

            if (input.Contains(" -m "))
            {
                string foundMode = input.Substring(input.IndexOf(" -m ") + " -m ".Length);
                input = input.Substring(0, input.IndexOf(" -m "));
                switch (foundMode.ToLower())
                {
                    case "standard":
                        mode = OsuMode.Standard;
                        break;
                    case "mania":
                        mode = OsuMode.Mania;
                        break;
                    case "ctb":
                    case "catch":
                        mode = OsuMode.Catch;
                        break;
                    case "taiko":
                        mode = OsuMode.Taiko;
                        break;
                    default:
                        return (foundMode + " is not a valid gamemode.").Humanize();
                }
            }
            
            string username = HttpUtility.HtmlEncode(input); // Test value

            //List<OsuUser> users = APIHelper<List<OsuUser>>.GetData(apiUrl + "get_user?u&k="+apiKey+"&u=" + username);
            OsuUser user = OsuApi.GetUser(username, mode);
            if (user==null)
                return "No user has been found.";
            else
            {
                EmbedFieldBuilder topplayField = new EmbedFieldBuilder
                {
                    Name = "Top plays",
                    Value = "No top plays available",
                    IsInline = false
                };

                List<OsuPlay> topPlays = OsuApi.GetUserBest(user, mode, 3, true);
                if (topPlays != null && topPlays.Count > 0)
                {
                    string topPlayString = "";
                    int i = 0;
                    topPlays.ForEach(play =>
                    {
                        i++;
                        string stringified = "**" + i + ". " + play.GetInfo();
                        topPlayString += stringified + "\n";
                    });
                    topplayField.Value = topPlayString;
                }

                EmbedBuilder embed = new EmbedBuilder()
                {
                    Color = ParentManager.bot.BotColor,
                    Title = "Profile of "+user.Name,
                    ThumbnailUrl = "https://a.ppy.sh/"+user.ID,
                    Description = "Showing osu!"+mode+" statistics",
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Rank",
                            Value = 
                                user.Performance==0?
                                    "No PP or inactive":
                                    ("#" + user.Globalrank.FormatNumber() + " (:flag_" + user.CountryCode.ToLower() + ": #" + user.Countryrank.FormatNumber() + ")"),
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Performance",
                            Value = Math.Round(user.Performance).FormatNumber()+"pp",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Level",
                            Value = Math.Floor(user.Level)+" ("+Math.Round((Math.Round(user.Level, 2)-Math.Floor(user.Level))*100)+"% to level "+ (Math.Ceiling(user.Level))+")",
                            IsInline = true
                        },
                        EmbedHelper.BlankField,
                        new EmbedFieldBuilder()
                        {
                            Name = "Ranked Score",
                            Value = user.RankedScore.FormatNumber(),
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Total Score",
                            Value = user.TotalScore.FormatNumber(),
                            IsInline = true
                        },
                        EmbedHelper.BlankField,
                        new EmbedFieldBuilder()
                        {
                            Name = "Playtime",
                            Value = TimeSpan.FromSeconds(user.Playtime).Humanize(maxUnit: TimeUnit.Hour),
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Maps played",
                            Value = user.Playcount.FormatNumber(),
                            IsInline = true
                        },
                        EmbedHelper.BlankField,
                        topplayField
                    },
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "User registered "+DateTime.Parse(user.Joindate).Humanize()
                    }
                };

                return embed;
            }
        }
    }
}