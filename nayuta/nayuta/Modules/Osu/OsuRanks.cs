using System.Collections.Generic;
using Discord;

namespace nayuta.Modules.Osu
{
    public class OsuRanks
    {
        public static Dictionary<string, string> RankingEmotes = new Dictionary<string, string>()
        {
            {"XH", ":SSH:810642742757163018"},
            {"X", ":SS:810642742698311740"},
            {"SH", ":SH:810642742626484274"},
            {"S", ":S_:810642742584934461"},
            {"A", ":A_:810642742598172763"},
            {"B", ":B_:810642742631727134"},
            {"C", ":C_:810642742568943687"},
            {"D", ":D_:810642742618619944"},
            {"F", ":D_:810642742618619944"},
        };

        public static Discord.Emote GetEmojiFromRank(string rank)
        {
            Discord.Emote emote = Emote.Parse("<"+RankingEmotes[rank]+">");
            return emote;
        }
    }
}