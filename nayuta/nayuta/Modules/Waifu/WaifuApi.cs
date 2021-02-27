using System.Collections.Generic;
using System.Web;

namespace nayuta.Modules.Waifu
{
    public class WaifuApi
    {
        private static string apiUrl = "https://www.mywaifu.net/api.php";

        public static List<Waifu> GetWaifus(string query)
        {
            WaifuResult<List<Waifu>> result = APIHelper<WaifuResult<List<Waifu>>>.GetData(apiUrl + "?apikey="+APIKeys.WaifuAPIKey+"&type=waifu&q="+HttpUtility.HtmlEncode(query));

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
    }
}