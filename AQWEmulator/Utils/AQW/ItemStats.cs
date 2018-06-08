using System.Collections.Generic;
using System.Linq;

namespace AQWEmulator.Utils.AQW
{
    public static class ItemStats
    {
        public static Dictionary<string, int> Get(double iBudget, Dictionary<string, int> statPattern)
        {
            var itemStats = new Dictionary<string, int>
            {
                {"END", 0},
                {"STR", 0},
                {"INT", 0},
                {"DEX", 0},
                {"WIS", 0},
                {"LCK", 0}
            };
            var T = 0.0D;
            var count = 0;
            foreach (var sta in itemStats.Keys.ToArray())
            {
                var dKey = (int) iBudget * statPattern[sta] / 100;
                itemStats[sta] = dKey;
                T += dKey;
            }

            var keys = itemStats.Keys.ToArray();
            while (T < iBudget)
            {
                itemStats[keys[count]] = itemStats[keys[count]] + 1;
                ++T;
                ++count;
                if (count > keys.Length - 1)
                    count = 0;
            }

            return itemStats;
        }
    }
}