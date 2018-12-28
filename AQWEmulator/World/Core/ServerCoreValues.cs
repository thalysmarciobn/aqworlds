using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using AQWEmulator.Utils;
using AQWEmulator.Xml;
using AQWEmulator.Xml.Game.Core;

namespace AQWEmulator.World.Core
{
    public static class ServerCoreValues
    {
        
        public static readonly XmlSettingsSerializer<ExpTable> ExpTable = new XmlSettingsSerializer<ExpTable>();
        
        public static readonly XmlSettingsSerializer<Values> Values = new XmlSettingsSerializer<Values>();
        
        public static readonly XmlSettingsSerializer<Settings> Settings = new XmlSettingsSerializer<Settings>();

        public static Dictionary<string, string> FlashSettings()
        {
            return new Dictionary<string, string>()
            {
                {"gMenu", Settings.Configuration.GMenu},
                {"sAssets", Settings.Configuration.SAssets},
                {"sBook", Settings.Configuration.SBook},
                {"sMap", Settings.Configuration.SMap},
                {"sNews", Settings.Configuration.SNews},
                {"sWTSandbox", Settings.Configuration.SWtSandbox.ToString()}
            };
        }

        public static Dictionary<string, double> CoreValues()
        {
            return new Dictionary<string, double>()
            {
                {"baseBlock", Values.Configuration.BaseBlock},
                {"baseBlockValue", Values.Configuration.BaseBlockValue},
            };
        }

        public static double GetManaByLevel(int level)
        {
            var _base = Values.Configuration.PCmpBase1;
            var delta = Values.Configuration.PCmpBase100;
            var curve = Values.Configuration.CurveExponent + _base / delta;
            return GetBaseValueByLevel(_base, delta, curve, level);
        }

        public static double GetHealthByLevel(int level)
        {
            var _base = Values.Configuration.PChpGoal1;
            var delta = Values.Configuration.PChpGoal100;
            var curve = 1.5D + _base / delta;
            return GetBaseValueByLevel(_base, delta, curve, level);
        }

        public static double GetBaseHPByLevel(int level)
        {
            var _base = Values.Configuration.PChpBase1;
            var curve = Values.Configuration.CurveExponent;
            var delta = Values.Configuration.PChpDelta;
            return GetBaseValueByLevel(_base, delta, curve, level);
        }

        public static double GetIBudget(int itemLevel, int iRty)
        {
            var GstBase = Values.Configuration.GstBase;
            var GstGoal = Values.Configuration.GstGoal;
            var statsExponent = Values.Configuration.StatsExponent;
            var rarity = iRty < 1.0D ? 1.0D : iRty;
            var level = itemLevel + rarity - 1.0D;
            var delta = GstGoal - GstBase;
            return GetBaseValueByLevel(GstBase, delta, statsExponent, level);
        }

        public static double GetInnateStats(int userLevel)
        {
            var PCstBase = Values.Configuration.PCstBase;
            var PCstGoal = Values.Configuration.PCstGoal;
            var statsExponent = Values.Configuration.StatsExponent;
            var delta = PCstGoal - PCstBase;
            return GetBaseValueByLevel(PCstBase, delta, statsExponent, userLevel);
        }

        public static double GetBaseValueByLevel(double _base, double delta, double curve, double userLevel)
        {
            var levelCap = Values.Configuration.LevelCap;
            var level = userLevel < 1 ? 1 : (userLevel > levelCap ? levelCap : userLevel);
            var x = (level - 1) / (levelCap - 1);
            return _base + Math.Pow(x, curve) * delta;
        }
    }
}