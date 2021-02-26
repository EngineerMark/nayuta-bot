using System;
using Newtonsoft.Json;

namespace nayuta.Modules.Waifu
{
    [Serializable]
    public struct WaifuMeasurements
    {
        public int? Bust { get; set; }
        public int? Waist { get; set; }
        public int? Hip { get; set; }

        public bool Complete => (Bust != null && Waist != null && Hip != null) && (Bust!=0 && Waist!=0 && Hip!=0);
    }
}