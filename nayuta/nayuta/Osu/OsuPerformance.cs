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
            if ((Play.Mods & OsuMods.ScoreV2) != 0)
                return 0f;
            
            switch (Play.Mode)
            {
                case OsuMode.Catch:
                    return CalculateCatchPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
                case OsuMode.Mania:
                    return CalculateManiaPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
                case OsuMode.Taiko:
                    return CalculateTaikoPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
                    return 0f;
                case OsuMode.Standard:
                    return CalculateStandardPP(combo, c50, c100, c300, cMiss, cKatu, cGeki);
            }

            return 0f;
        }

        public float CalculateTaikoPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;
            
            float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;
            
            float strain = Mathf.Pow(5.0f*Mathf.Max(1.0f, (float)Beatmap.Starrating/0.0075f)-4.0f, 2.0f)/100000.0f;

            float bonusLength = 1 + 0.1f * Mathf.Min(1.0f, Beatmap.ObjectCount / 1500f);
            strain *= bonusLength;

            strain *= Mathf.Pow(0.985f, cMiss);

            if ((Play.Mods & OsuMods.Hidden) != 0)
                strain *= 1.025f;
            
            if ((Play.Mods & OsuMods.Flashlight) != 0)
                strain *= 1.05f*bonusLength;

            strain *= real_acc;

            float scaleOD = Beatmap.MapStats.OD;
            if ((Play.Mods & OsuMods.Easy) != 0)
                scaleOD *= 0.5f;

            if ((Play.Mods & OsuMods.HardRock) != 0)
                scaleOD *= 1.4f;

            scaleOD = Mathf.Max(Mathf.Min(scaleOD, 10f), 0f);

            float hwMax = 20;
            float hwMin = 50;
            float hwRes = hwMin + (hwMax - hwMin) * scaleOD / 10f;
            hwRes = Mathf.Floor(hwRes) - 0.5f;

            if ((Play.Mods & OsuMods.HalfTime) != 0)
                hwRes *= 1.5f;

            if ((Play.Mods & OsuMods.DoubleTime) != 0)
                hwRes *= 0.75f;

            hwRes = Mathf.Round(hwRes * 100) / 100;
            float OD300 = hwRes;

            float acc = 0;
            if (OD300 > 0)
            {
                acc = Mathf.Pow(150.0f / OD300, 1.1f) * Mathf.Pow(real_acc, 15f) * 22.0f;
                acc *= Mathf.Min(1.15f, Mathf.Pow((float)Beatmap.ObjectCount / 1500.0f, 0.3f));
            }

            float total = 1.1f;
            
            if ((Play.Mods & OsuMods.NoFail) != 0)
                total *= 0.9f;
            
            if ((Play.Mods & OsuMods.Hidden) != 0)
                total *= 1.1f;

            return Mathf.Pow(
                    Mathf.Pow(strain, 1.1f)+Mathf.Pow(acc, 1.1f),
                    1.0f/1.1f
                ) * total;
        }

        public float CalculateManiaPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;

            float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki);

            float strainbase = Mathf.Pow(5.0f * Mathf.Max(1f, (float) Beatmap.Starrating / 0.2f) - 4f, 2.2f) / 135.0f;
            strainbase *= 1f + 0.1f * Mathf.Min(1f, (float) Beatmap.ObjectCount / 1500f);

            float strain = strainbase;
            float scoreMultiplier = 0;
            if (Play.Score < 500000)
                scoreMultiplier = Play.Score / 500000f * 0.1f;
            else if (Play.Score < 600000)
                scoreMultiplier = (Play.Score - 500000f) / 100000f * 0.3f;
            else if (Play.Score < 700000)
                scoreMultiplier = (Play.Score - 600000f) / 100000f * 0.25f+0.3f;
            else if (Play.Score < 800000)
                scoreMultiplier = (Play.Score - 700000f) / 100000f * 0.2f+0.55f;
            else if (Play.Score < 900000)
                scoreMultiplier = (Play.Score - 800000f) / 100000f * 0.15f+0.75f;
            else
                scoreMultiplier = (Play.Score - 900000f) / 100000f * 0.1f+0.9f;
            

            // float[][] odconvertwindow = new float[2][]
            // {
            //     new float[2] {47, 34},
            //     new float[2] {65, 47}
            // };
            //
            // float odwindow = ((Beatmap.OriginalMode == OsuMode.Standard)) ? odconvertwindow[((Play.Mods&OsuMods.Easy)!=0)?1:0][(Beatmap.MapStats.OD>=5)?1:0]:((64f-3f*Beatmap.MapStats.OD)* (((Play.Mods & OsuMods.Easy) != 0) ? 1.4f : 1f));
            //
            // float acc = Mathf.Max(0.0f, 0.2f - ((odwindow - 34f) * 0.006667f)) * strain;
            // acc *= Mathf.Pow(Mathf.Max(0.0f, (Play.Score - 960000f)) / 40000.0f, 1.1f);

            float acc = 1.0f;
            float nerfod = ((Play.Mods & OsuMods.Easy) != 0) ? 0.5f : 1.0f;
            if (Play.Score >= 960000)
                acc = Beatmap.MapStats.OD * nerfod * 0.02f * strainbase *
                      Mathf.Pow((Play.Score - 960000f) / 40000f, 1.1f);
            else
                acc = 0;

            float total = 0.73f;

            if ((Play.Mods & OsuMods.NoFail) != 0)
                total *= 0.9f;
            
            if ((Play.Mods & OsuMods.SpunOut) != 0)
                total *= 0.95f;
            
            if ((Play.Mods & OsuMods.Easy) != 0)
                total *= 0.5f;
            
            return total*Mathf.Pow(
                Mathf.Pow(strain*scoreMultiplier, 1.1f)+
                Mathf.Pow(acc, 1.1f),
                1.0f/1.1f
                )*1.1f;
        }

        public float CalculateCatchPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;

            float real_acc = OsuApi.CalculateAccuracy(Play.Mode, cMiss, c50, c100, c300, cKatu, cGeki) * 0.01f;

            float value = Mathf.Pow(5.0f*Mathf.Max(1.0f, (float)Beatmap.Starrating/0.0049f)-4.0f, 2.0f)/100000.0f;
            float bonusLength = 0.95f+0.3f*Mathf.Min(1.0f, Beatmap.ObjectCount/2500f)+
                                (Beatmap.ObjectCount>2500?Mathf.Log10(Beatmap.ObjectCount/2500f)*0.475f:0.0f);
            value *= bonusLength;

            //Miss penalty
            value *= Mathf.Pow(0.97f, cMiss);
            
            // Combo scaling
            if (Beatmap.MaxCombo > 0)
                value *= Mathf.Min(Mathf.Pow(Play.MaxCombo, 0.8f)/Mathf.Pow((float)Beatmap.MaxCombo, 0.8f), 1.0f);
            
            //AR bonus
            float bonusAR = 1.0f;
            if (Beatmap.MapStats.AR > 9.0f)
                bonusAR += 0.1f * (Beatmap.MapStats.AR - 9.0f);
            if (Beatmap.MapStats.AR > 10.0f)
                bonusAR += 0.1f * (Beatmap.MapStats.AR - 10.0f);
            else if (Beatmap.MapStats.AR < 8.0f)
                bonusAR += 0.025f * (8.0f - Beatmap.MapStats.AR);
            value *= bonusAR;
            
            //HD Bonus
            if ((Play.Mods & OsuMods.Hidden) != 0)
            {
                if (Beatmap.MapStats.AR <= 10.0f)
                    value *= 1.05f + 0.075f * (10.0f - Beatmap.MapStats.AR);
                else if (Beatmap.MapStats.AR > 10f)
                    value *= 1.01f + 0.04f * (11.0f - Mathf.Min(11.0f, Beatmap.MapStats.AR));
            }

            if ((Play.Mods & OsuMods.Flashlight) != 0)
                value *= 1.35f * bonusLength;

            value *= Mathf.Pow(real_acc, 5.5f);

            if ((Play.Mods & OsuMods.NoFail) != 0)
                value *= 0.9f;
            if ((Play.Mods & OsuMods.SpunOut) != 0)
                value *= 0.95f;

            return value;
        }

        public float CalculateStandardPP(float combo, float c50, float c100, float c300, float cMiss, float cKatu = 0, float cGeki = 0)
        {
            if ((Play.Mods & OsuMods.Relax) != 0 || (Play.Mods & OsuMods.Relax2) != 0 ||
                (Play.Mods & OsuMods.Autoplay) != 0)
                return 0f;
            
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
            
            // Prehotfix bonus AR (<Jan 16 2021)
            // if (Beatmap.MapStats.AR > 10.33f)
            //     bonusAR += 0.4f * (Beatmap.MapStats.AR - 10.33f);
            // else if (Beatmap.MapStats.AR < 8.0f)
            //     bonusAR += 0.1f * (8.0f - Beatmap.MapStats.AR);
            
            // New AR bonus
            if(Beatmap.MapStats.AR>10.33f)
                bonusAR+=0.2666f*Mathf.Min(1.0f, 1f/1000f*Beatmap.ObjectCount);
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

            aim *= bonusAcc;

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
            float squaredOD = Mathf.Pow(Beatmap.MapStats.OD, 2f);
            float bonusOD = (0.95f + squaredOD / 750f) *
                            Mathf.Pow(real_acc, (14.5f - Mathf.Max(Beatmap.MapStats.OD, 8f)) / 2f);

            speed *= bonusOD;
            
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
        }

        private static float GetPPBase(float stars) =>
            Mathf.Pow(5.0f * Mathf.Max(1.0f, stars / 0.0675f) - 4.0f, 3.0f) / 100000.0f;
    }
}