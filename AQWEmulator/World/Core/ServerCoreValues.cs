using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AQWEmulator.Settings;
using AQWEmulator.Utils;
using AQWEmulator.Utils.Files;

namespace AQWEmulator.World.Core
{
    public static class ServerCoreValues
    {
        private static readonly ConcurrentDictionary<string, string> _settings =
            new ConcurrentDictionary<string, string>();

        private static readonly ConcurrentDictionary<string, double> _core =
            new ConcurrentDictionary<string, double>();

        private static readonly ConcurrentDictionary<int, CoreExpToLevel> _levelExp =
            new ConcurrentDictionary<int, CoreExpToLevel>();

        public static ServerSettings Settings { get; private set; }

        public static void LoadSettings(ServerSettings settings)
        {
            Settings = settings;
        }

        public static void LoadCoreSettings(string path, string name, string value)
        {
            foreach (var core in Xml.Values(path, value))
                lock (_settings)
                {
                    if (core.Attribute(name) != null)
                        _settings.TryAdd(core.Attribute(name).Value, core.Value);
                }
        }

        public static void LoadCoreValues(string path, string name, string value)
        {
            foreach (var core in Xml.Values(path, value))
                lock (_core)
                {
                    if (core.Attribute(name) != null)
                        _core.TryAdd(core.Attribute(name).Value, Convert.ToDouble(core.Value));
                }
        }

        public static void LoadCoreLevels(string path, string level, string rate, string value)
        {
            foreach (var core in Xml.Values(path, value))
                lock (_levelExp)
                {
                    if (core.Attribute(level) != null && core.Attribute(rate) != null)
                        _levelExp.TryAdd(Convert.ToInt32(core.Attribute(level).Value),
                            new CoreExpToLevel(Convert.ToInt32(core.Attribute(level).Value),
                                Convert.ToDouble(core.Attribute(rate).Value), Convert.ToInt32(core.Value)));
                }
        }

        public static Dictionary<string, string> FlashSettings()
        {
            var settings = new Dictionary<string, string>();
            lock (_settings)
            {
                foreach (var kvp in _settings) settings.Add(kvp.Key, kvp.Value);
            }

            return settings;
        }

        public static Dictionary<string, double> Core()
        {
            var core = new Dictionary<string, double>();
            lock (_core)
            {
                foreach (var kvp in _core) core.Add(kvp.Key, kvp.Value);
            }

            return core;
        }

        public static double Get(string index)
        {
            lock (_core)
            {
                if (_core.TryGetValue(index, out var _value)) return _value;
            }

            return 0;
        }

        public static CoreExpToLevel GetExpToLevel(int playerLevel)
        {
            lock (_levelExp)
            {
                if (_levelExp.TryGetValue(playerLevel, out var coreExpToLevel)) return coreExpToLevel;
            }

            return null;
        }

        public static double GetManaByLevel(int level)
        {
            var _base = Get(Indent.Core.PcMpBase1);
            var delta = Get(Indent.Core.PcMpBase100);
            var curve = Get(Indent.Core.CurveExponent) + _base / delta;
            return GetBaseValueByLevel(_base, delta, curve, level);
        }

        public static double GetHealthByLevel(int level)
        {
            var _base = Get(Indent.Core.PcHpGoal1);
            var delta = Get(Indent.Core.PcHpGoal100);
            var curve = 1.5D + _base / delta;
            return GetBaseValueByLevel(_base, delta, curve, level);
        }

        public static double GetBaseHPByLevel(int level)
        {
            var _base = Get(Indent.Core.PcHpBase1);
            var curve = Get(Indent.Core.CurveExponent);
            var delta = Get(Indent.Core.PcHpDelta);
            return GetBaseValueByLevel(_base, delta, curve, level);
        }

        public static double GetIBudget(int itemLevel, int iRty)
        {
            var GstBase = Get(Indent.Core.GstBase);
            var GstGoal = Get(Indent.Core.GstGoal);
            var statsExponent = Get(Indent.Core.StatsExponent);
            var rarity = iRty < 1.0D ? 1.0D : iRty;
            var level = itemLevel + rarity - 1.0D;
            var delta = GstGoal - GstBase;
            return GetBaseValueByLevel(GstBase, delta, statsExponent, level);
        }

        public static double GetInnateStats(int userLevel)
        {
            var PCstBase = Get(Indent.Core.PcStBase);
            var PCstGoal = Get(Indent.Core.PcStGoal);
            var statsExponent = Get(Indent.Core.StatsExponent);
            var delta = PCstGoal - PCstBase;
            return GetBaseValueByLevel(PCstBase, delta, statsExponent, userLevel);
        }

        public static double GetBaseValueByLevel(double _base, double delta, double curve, double userLevel)
        {
            var levelCap = Get(Indent.Core.LevelCap);
            var level = userLevel < 1 ? 1 : (userLevel > levelCap ? levelCap : userLevel);
            var x = (level - 1) / (levelCap - 1);
            return _base + Math.Pow(x, curve) * delta;
        }
    }
}