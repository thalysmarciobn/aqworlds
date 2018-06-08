namespace AQWEmulator.Utils
{
    public class RoundTens
    {
        public static int Parse(int value)
        {
            for (var i = 0; i < 9 && value % 10 != 0; ++i) ++value;
            return value;
        }
    }
}