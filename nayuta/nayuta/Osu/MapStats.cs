using System;
using nayuta.Math;

namespace nayuta.Osu
{
    public class MapStats
    {
        private const float OD0Ms = 79.5f;
        private const float OD10Ms = 19.5f;
        private const float AR0Ms = 1800.0f;
        private const float AR5Ms = 1200.0f;
        private const float AR10Ms = 450.0f;

        private const float ODMsStep = (OD0Ms - OD10Ms) / 10.0f;
        private const float ARMsStep1 = (AR0Ms - AR5Ms) / 5.0f;
        private const float ARMsStep2 = (AR5Ms - AR10Ms) / 5.0f;
        
        public float AR, OD, CS, HP;
        public float Speed;

        public OsuBeatmap Beatmap;
        public OsuMods Mods;

        public MapStats(OsuBeatmap beatmap, OsuMods mods)
        {
            Beatmap = beatmap;
            Mods = mods;

            Calculate();
        }

        public void Calculate()
        {
            Speed = 1.0f;
            float odArLimit = 10;

            if ((Mods & (OsuMods.DoubleTime | OsuMods.Nightcore)) != 0 && (Mods&OsuMods.HardRock)!=0)
                odArLimit = 11;

            if ((Mods & (OsuMods.DoubleTime | OsuMods.Nightcore)) != 0)
                Speed *= 1.5f;

            if ((Mods&(OsuMods.HalfTime))!=0)
                Speed *= 0.75f;

            float odArHpMultiplier = 1.0f;

            if ((Mods & OsuMods.HardRock) != 0)
                odArHpMultiplier *= 1.4f;
            
            if ((Mods & OsuMods.Easy) != 0)
                odArHpMultiplier *= 0.5f;

            AR = Beatmap.ApproachRate??0;
            AR *= odArHpMultiplier;

            float arms = AR < 5f ? AR0Ms - ARMsStep1 * AR : AR5Ms - ARMsStep2 * (AR - 5f);
            AR = (arms > AR5Ms) ? (AR0Ms - arms) / ARMsStep1 : 5.0f + (AR5Ms - arms) / ARMsStep2;

            OD = Beatmap.OverallDifficulty??0;
            OD *= odArHpMultiplier;
            float odms = OD0Ms - (float) Mathf.Ceil(ODMsStep * OD);
            odms = (float) Mathf.Min(OD0Ms, Mathf.Max(OD10Ms, odms));
            odms /= Speed;
            OD = (float) ((OD0Ms - odms) / ODMsStep);

            CS = Beatmap.CircleSize??0;

            if ((Mods & OsuMods.HardRock) != 0)
                CS *= 1.3f;

            if ((Mods & OsuMods.Easy) != 0)
                CS *= 0.5f;

            CS = Mathf.Min(10.0f, CS);
            HP = Mathf.Min(10f, Beatmap.Drain??0 * odArHpMultiplier);

            AR = Mathf.Min(odArLimit, AR);
            OD = Mathf.Min(odArLimit, OD);
        }
    }
}