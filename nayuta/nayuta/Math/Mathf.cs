namespace nayuta.Math
{
    public static class Mathf
    {
        public static float Round(float a, int b = 0) => (float) System.Math.Round(a, b);
        
        public static float Min(float a, float b) => (float) System.Math.Min(a, b);

        public static float Max(float a, float b) => (float) System.Math.Max(a, b);

        public static float Ceil(float a) => (float) System.Math.Ceiling(a);

        public static float Floor(float a) => (float) System.Math.Floor(a);

        public static float Pow(float a, float b) => (float) System.Math.Pow(a, b);
        
        public static float Abs(float a) => (float) System.Math.Abs(a);

        public static float Log10(float a) => (float) System.Math.Log10(a);

        public static string FormatNumber(this float value) => ((double) value).FormatNumber();
    }
}