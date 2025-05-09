namespace Terra.Extensions
{
    public static class NumericExtensions
    {
        public static float ToFactor(this int value)
        {
            return value * 0.01f;
        }

        public static float ToFactor(this float value)
        {
            return value * 0.01f;
        }

        public static double ToFactor(this double value)
        {
            return value * 0.01;
        }

        public static decimal ToFactor(this decimal value)
        {
            return value * 0.01m;
        }
    }
}