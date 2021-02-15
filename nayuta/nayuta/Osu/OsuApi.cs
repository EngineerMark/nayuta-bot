using System.Collections.Generic;

namespace nayuta.Osu
{
    public static class OsuApi
    {
        private static string apiKey = "c8186625bb3684645cd6e46325fe17c59537e54c";
        private static string apiUrl = "https://osu.ppy.sh/api/";

        public static OsuUser GetUser(string username, OsuMode mode = OsuMode.Standard)
        {
            List<OsuUser> users = APIHelper<List<OsuUser>>.GetData(apiUrl + "get_user?k="+apiKey+"&u=" + username + "&m="+(int)mode);
            return users.Count > 0 ? users[0] : null;
        }

        public static List<OsuPlay> GetUserBest(OsuUser user, OsuMode mode = OsuMode.Standard, int limit = 5, bool generateBeatmaps = false)
        {
            return GetUserBest(user.Name, mode, limit, generateBeatmaps);
        }
        
        public static List<OsuPlay> GetUserBest(string username, OsuMode mode = OsuMode.Standard, int limit = 5, bool generateBeatmaps = false)
        {
            List<OsuPlay> plays = APIHelper<List<OsuPlay>>.GetData(apiUrl+"get_user_best?k="+apiKey+"&u=" + username + "&m="+(int)mode + "&limit="+limit);
            plays.ForEach(play=>
            {
                play.Mode = mode;
                if (generateBeatmaps)
                    play.Beatmap = GetBeatmap(play.MapID, play.Mods, mode);
            });
            return plays.Count>0?plays:null;
        }

        public static OsuBeatmap GetBeatmap(string id, OsuMods mods = OsuMods.None, OsuMode mode = OsuMode.Standard)
        {
            string url = apiUrl + "get_beatmaps?k=" + apiKey + "&b=" + id + "&m=" + (int) mode + "&a=1&mods=" + (int)mods.ModParser(true);
            List<OsuBeatmap> maps = APIHelper<List<OsuBeatmap>>.GetData(url, true);
            return maps.Count>0?maps[0]:null;
        }

        public static OsuMods ModParser(this OsuMods mods, bool forApi = false)
        {
            OsuMods _mods = mods;
            //Stuff like NC has DT aswell, we gotta filter stuff like that
            if (!forApi)
            {
                if ((mods & OsuMods.Nightcore)==OsuMods.Nightcore) _mods &= ~OsuMods.DoubleTime;
            }
            else
            {
                //If target is API usage, we can only have several mods available
                _mods &= OsuMods.APIMods;
            }

            return _mods;
        }
        
        public static OsuModsShort ModParser(this OsuModsShort mods)
        {
            return (OsuModsShort)((OsuMods) mods).ModParser();
        }
    }
}