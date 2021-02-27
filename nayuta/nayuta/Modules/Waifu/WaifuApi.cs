using System.Collections.Generic;
using System.Web;
using Discord;
using nayuta.Internal;

namespace nayuta.Modules.Waifu
{
    public class WaifuApi
    {
        private static string apiUrl = "https://www.mywaifu.net/api.php";

        public static BetterList<Waifu> GetWaifus(string query)
        {
            WaifuResult<BetterList<Waifu>> result = APIHelper<WaifuResult<BetterList<Waifu>>>.GetData(apiUrl + "?apikey="+APIKeys.WaifuAPIKey+"&type=waifu&q="+HttpUtility.HtmlEncode(query));

            if (result.Code == "200")
            {
                if (result.Data.Count > 0)
                {
                    for (int i = 0; i < result.Data.Count; i++)
                    {
                        Waifu _w = GetWaifu(result.Data[i].ID);
                        if (_w != null)
                            result.Data[i] = _w;
                    }
                }                
                return result.Data;
            }
            return null;
        }

        public static Waifu GetWaifu(int ID)
        {
            WaifuResult<List<Waifu>> result = APIHelper<WaifuResult<List<Waifu>>>.GetData(apiUrl + "?apikey="+APIKeys.WaifuAPIKey+"&type=waifudata&q="+HttpUtility.HtmlEncode(ID));
            
            if (result.Code == "200" && result.Data.Count>0)
                return result.Data[0];
            return null;
        }

        public static User GetUser(int ID)
        {
            WaifuResult<User> result = APIHelper<WaifuResult<User>>.GetData(apiUrl + "?apikey="+APIKeys.WaifuAPIKey+"&type=user&q="+HttpUtility.HtmlEncode(ID));

            if (result.Code == "200" && result.Data != null)
                return result.Data;
            return null;
        }

        public static object GetWaifuEmbed(Waifu waifu)
        {
            if (waifu == null)
                return null;
            
            User uploader = WaifuApi.GetUser(waifu.SubmitterID);
            string noValue = "\uD83D\uDEC7";

            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = waifu.Name,
                Description = waifu.SourceName,
                Url = "https://www.mywaifu.net/waifu?id="+waifu.ID,
                ImageUrl = "https://www.mywaifu.net/api.php?type=thumbnail&q="+waifu.ID,
                Fields = new List<EmbedFieldBuilder>()
                {
                    new EmbedFieldBuilder()
                    {
                        Name = "Bust",
                        Value = waifu.Measurements.Bust==0?noValue:waifu.Measurements.Bust+"cm",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Waist",
                        Value = waifu.Measurements.Waist==0?noValue:waifu.Measurements.Waist+"cm",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Hip",
                        Value = waifu.Measurements.Hip==0?noValue:waifu.Measurements.Hip+"cm",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Age",
                        Value = waifu.Age==0?noValue:waifu.Age,
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Height",
                        Value = waifu.Height==0?noValue:waifu.Height+"cm",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Weight",
                        Value = waifu.Weight==0?noValue:waifu.Weight+"kg",
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Birthday",
                        Value = waifu.Birthday.Length==0?noValue:waifu.Birthday,
                        IsInline = true
                    },
                    new EmbedFieldBuilder()
                    {
                        Name = "Bloodtype",
                        Value = waifu.Bloodtype.Length==0?noValue:waifu.Bloodtype,
                        IsInline = true
                    },
                    EmbedHelper.BlankField
                },
                Footer = new EmbedFooterBuilder()
                {
                    Text = "Submitted by "+uploader.Username+" on "+string.Format("{0}", waifu.UploadTime.DateTime.ToString("MMMM dd, yyyy")),
                    IconUrl = uploader.Thumbnail
                }
            };

            return embed;
        }
    }
}