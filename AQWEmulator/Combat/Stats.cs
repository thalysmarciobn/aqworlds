using System.Collections.Generic;

namespace AQWEmulator.Combat
{
    public static class Stats
    {
        private static readonly int[] ClassPointsRank = {900, 3600, 10000, 22500, 44100, 78400, 129600, 202500, 302500};
        private static readonly string[] StatsOrder = {"STR", "END", "DEX", "INT", "WIS", "LCK"};

        private static readonly Dictionary<string, double> RatiosBySlot = new Dictionary<string, double>
        {
            {"he", 0.25},
            {"ar", 0.25},
            {"ba", 0.2},
            {"Weapon", 0.33}
        };

        private static readonly Dictionary<string, Dictionary<string, double>> ClassCatMap =
            new Dictionary<string, Dictionary<string, double>>
            {
                {
                    "M1",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.27},
                        {"END", 0.3},
                        {"DEX", 0.22},
                        {"INT", 0.05},
                        {"WIS", 0.1},
                        {"LCK", 0.06}
                    })
                },
                {
                    "M2",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.2},
                        {"END", 0.22},
                        {"DEX", 0.33},
                        {"INT", 0.05},
                        {"WIS", 0.1},
                        {"LCK", 0.1}
                    })
                },
                {
                    "M3",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.24},
                        {"END", 0.2},
                        {"DEX", 0.2},
                        {"INT", 0.24},
                        {"WIS", 0.07},
                        {"LCK", 0.05}
                    })
                },
                {
                    "M4",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.3},
                        {"END", 0.18},
                        {"DEX", 0.3},
                        {"INT", 0.02},
                        {"WIS", 0.06},
                        {"LCK", 0.14}
                    })
                },
                {
                    "C1",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.06},
                        {"END", 0.2},
                        {"DEX", 0.11},
                        {"INT", 0.33},
                        {"WIS", 0.15},
                        {"LCK", 0.15}
                    })
                },
                {
                    "C2",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.08},
                        {"END", 0.27},
                        {"DEX", 0.1},
                        {"INT", 0.3},
                        {"WIS", 0.1},
                        {"LCK", 0.15}
                    })
                },
                {
                    "C3",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.06},
                        {"END", 0.23},
                        {"DEX", 0.05},
                        {"INT", 0.28},
                        {"WIS", 0.28},
                        {"LCK", 0.15}
                    })
                },
                {
                    "S1",
                    new Dictionary<string, double>(new Dictionary<string, double>
                    {
                        {"STR", 0.22},
                        {"END", 0.18},
                        {"DEX", 0.21},
                        {"INT", 0.08},
                        {"WIS", 0.08},
                        {"LCK", 0.23}
                    })
                }
            };

        public static Dictionary<string, double> GetClassCatList(string category)
        {
            return ClassCatMap[category];
        }

        public static IEnumerable<string> GetStats()
        {
            return StatsOrder;
        }

        public static double GetRatiosBySlot(string slot)
        {
            return RatiosBySlot[slot];
        }

        public static int GetRankFromPoints(int cp)
        {
            for (var i = 1; i < ClassPointsRank.Length; ++i)
                if (cp < ClassPointsRank[i])
                    return i;
            return 10;
        }
    }
}