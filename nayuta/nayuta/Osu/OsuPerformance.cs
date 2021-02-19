using System;
using nayuta.Math;

namespace nayuta.Osu
{
    /// <summary>
    /// Base class for PP handling
    /// </summary>
    public class OsuPerformance
    {
        public OsuBeatmap Beatmap;
        public OsuPlay Play;

        public float CurrentValue { get; set; }
        public float CurrentValueIfFC { get; set; }

        public float AimPP = 0;
        public float SpeedPP = 0;
        public float AccPP = 0;

        public OsuPerformance(OsuPlay play, OsuBeatmap beatmap)
        {
            Beatmap = beatmap;
            Play = play;
            
            CalculateCurrentPerformance();
        }

        public void CalculateCurrentPerformance()
        {
            if (Play.PP == -1)
                CurrentValue = CalculatePerformance(Play.MaxCombo, Play.C50, Play.C100, Play.C300, Play.CMiss, Play.CKatu, Play.CGeki);
            else
                CurrentValue = Play.PP;

            CurrentValueIfFC =
                CalculatePerformance(Beatmap.MaxCombo??0, Play.C50, Play.C100, Play.C300 + Play.CMiss, 0, Play.CKatu, Play.CGeki);
        }

        public float CalculatePerformance(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            switch (Play.Mode)
            {
                case OsuMode.Catch:
                case OsuMode.Mania:
                case OsuMode.Taiko:
                    //Tbd
                    return 0f;
                    break;
                case OsuMode.Standard:

                    float _objectsOver2K = Beatmap.ObjectCount / 2000f;
                    float bonusLength = 0.95f + 0.4f * Mathf.Min(1.0f, _objectsOver2K);

                    if (_objectsOver2K > 2000)
                        bonusLength += Mathf.Log10(_objectsOver2K) * 0.5f;

                    float _comboBreak = Mathf.Pow(combo, 0.8f) / Mathf.Pow((float)Beatmap.MaxCombo, 0.8f);
                    
                    float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;

                    float doubletapPenalty = Mathf.Pow(0.98f,
                        c50 < Beatmap.ObjectCount ? 0 : c50 - Beatmap.ObjectCount / 500f);

                    // AR bonus
                    float bonusAR = 0.0f;
                    if (Beatmap.MapStats.AR > 10.33f)
                        bonusAR += 0.4f * (Beatmap.MapStats.AR - 10.33f);
                    else if (Beatmap.MapStats.AR < 8.0f)
                        bonusAR += 0.1f * (8.0f - Beatmap.MapStats.AR);

                    bonusAR = 1.0f + Mathf.Min(bonusAR, bonusAR * (Beatmap.ObjectCount / 1000f));

                    float aim = GetPPBase((float)Beatmap.StarratingAim);
                    aim *= bonusLength;

                    //Miss penalty
                    if (cMiss > 0)
                        aim *= 0.97f * Mathf.Pow(1 - Mathf.Pow(cMiss / Beatmap.ObjectCount, 0.775f), cMiss);

                    aim *= _comboBreak;
                    aim *= bonusAR;
                    
                    // HD bonus
                    float bonusHD = 1.0f;
                    if ((Play.Mods & OsuMods.Hidden) != 0)
                        bonusHD *= 1.0f + 0.04f * (12.0f - Beatmap.MapStats.AR);

                    aim *= bonusHD;
                    
                    // FL bonus
                    if ((Play.Mods & OsuMods.Flashlight) != 0)
                    {
                        float bonusFL = 1.0f + 0.35f * Mathf.Min(1.0f, Beatmap.ObjectCount / 200f);
                        if (Beatmap.ObjectCount > 200f)
                            bonusFL += 0.3f * Mathf.Min(1.0f, (Beatmap.ObjectCount - 200f) / 300f);

                        if (Beatmap.ObjectCount > 500f)
                            bonusFL += (Beatmap.ObjectCount - 500f) / 1200f;

                        aim *= bonusFL;
                    }

                    float bonusAcc = 0.5f + real_acc / 2.0f;
                    float squaredOD = Mathf.Pow(Beatmap.MapStats.OD, 2f);
                    float bonusOD = 0.98f + squaredOD / 2500f;

                    aim *= bonusAcc;
                    aim *= bonusOD;

                    AimPP = aim;

                    float speed = GetPPBase((float)Beatmap.StarratingSpeed);
                    speed *= bonusLength;

                    if (cMiss > 0)
                        speed *= 0.97f * Mathf.Pow(1f - Mathf.Pow(cMiss / Beatmap.ObjectCount, 0.775f),
                            Mathf.Pow(cMiss, 0.875f));
                    speed *= doubletapPenalty;
                    speed *= _comboBreak;
                    if (Beatmap.MapStats.AR > 10.33f)
                        speed *= bonusAR;
                    speed *= bonusHD;
                    speed *= (0.95f + squaredOD / 750f) *
                             Mathf.Pow(real_acc, (14.5f - Mathf.Max(Beatmap.MapStats.OD, 8f)) / 2f);

                    SpeedPP = speed;
                    
                    float acc = (Mathf.Pow(1.52163f, Beatmap.MapStats.OD)*Mathf.Pow(real_acc, 24.0f)*2.83f);

                    acc *= Mathf.Min(1.15f, Mathf.Pow((float)Beatmap.CircleCount / 1000f, 0.3f));
                    if ((Play.Mods & OsuMods.Hidden) != 0)
                        acc *= 1.08f;
                    if ((Play.Mods & OsuMods.Flashlight) != 0)
                        acc *= 1.02f;
                    AccPP = acc;

                    float final_multiplier = 1.12f;
                    if ((Play.Mods & OsuMods.NoFail) != 0)
                        final_multiplier *= Mathf.Max(0.9f, 1.0f - 0.02f * cMiss);
                    if ((Play.Mods & OsuMods.SpunOut) != 0)
                        final_multiplier *= 1.0f - Mathf.Pow((float)Beatmap.SpinnerCount / (float)Beatmap.ObjectCount, 0.85f);

                    float total = Mathf.Pow(
                        Mathf.Pow(AimPP, 1.1f)+
                        Mathf.Pow(SpeedPP, 1.1f)+
                        Mathf.Pow(AccPP, 1.1f),
                        1.0f/1.1f
                    )*final_multiplier;
                    
                    return total;
                    
                    break;
            }

            return 0f;
        }

        private static float GetPPBase(float stars) =>
            (float)Mathf.Pow(5.0f * Mathf.Max(1.0f, stars / 0.0675f) - 4.0f, 3.0f) / 100000.0f;
    }
}