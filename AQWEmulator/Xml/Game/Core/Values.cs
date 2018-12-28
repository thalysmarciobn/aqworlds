using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game.Core
{
    [Serializable]
    public class Values
    {
        [XmlElement("BaseBlock")]
        public double BaseBlock { get; set; }
        
        [XmlElement("BaseBlockValue")]
        public double BaseBlockValue { get; set; }
        
        [XmlElement("BaseCritical")]
        public double BaseCritical { get; set; }
        
        [XmlElement("BaseCriticalValue")]
        public double BaseCriticalValue { get; set; }
        
        [XmlElement("BaseDodge")]
        public double BaseDodge { get; set; }
        
        [XmlElement("BaseEventValue")]
        public double BaseEventValue { get; set; }
        
        [XmlElement("BaseHaste")]
        public double BaseHaste { get; set; }
        
        [XmlElement("BaseHit")]
        public double BaseHit { get; set; }
        
        [XmlElement("BaseMiss")]
        public double BaseMiss { get; set; }
        
        [XmlElement("BaseParry")]
        public double BaseParry { get; set; }
        
        [XmlElement("BaseResistValue")]
        public double BaseResistValue { get; set; }
        
        [XmlElement("BigNumberBase")]
        public double BigNumberBase { get; set; }
        
        [XmlElement("CurveExponent")]
        public double CurveExponent { get; set; }
        
        [XmlElement("GstBase")]
        public double GstBase { get; set; }
        
        [XmlElement("GstGoal")]
        public double GstGoal { get; set; }
        
        [XmlElement("GstRatio")]
        public double GstRatio { get; set; }
        
        [XmlElement("APtoDPS")]
        public double ApToDps { get; set; }
        
        [XmlElement("HPperEND")]
        public double HpPerEnd { get; set; }
        
        [XmlElement("LevelCap")]
        public double LevelCap { get; set; }
        
        [XmlElement("LevelMax")]
        public double LevelMax { get; set; }
        
        [XmlElement("MPperWIS")]
        public double MpPerWis { get; set; }
        
        [XmlElement("SPtoDPS")]
        public double SpToDps { get; set; }
        
        [XmlElement("ModRating")]
        public double ModRating { get; set; }
        
        [XmlElement("PCDPSMod")]
        public double PcDpsMod { get; set; }
        
        [XmlElement("PChpBase1")]
        public double PChpBase1 { get; set; }
        
        [XmlElement("PChpBase100")]
        public double PChpBase100 { get; set; }
        
        [XmlElement("PChpDelta")]
        public double PChpDelta { get; set; }
        
        [XmlElement("PChpGoal1")]
        public double PChpGoal1 { get; set; }
        
        [XmlElement("PChpGoal100")]
        public double PChpGoal100 { get; set; }
        
        [XmlElement("PCmpBase1")]
        public double PCmpBase1 { get; set; }
        
        [XmlElement("PCmpBase100")]
        public double PCmpBase100 { get; set; }
        
        [XmlElement("PCmpDelta")]
        public double PCmpDelta { get; set; }
        
        [XmlElement("PCstBase")]
        public double PCstBase { get; set; }
        
        [XmlElement("PCstGoal")]
        public double PCstGoal { get; set; }
        
        [XmlElement("PCstRatio")]
        public double PCstRatio { get; set; }
        
        [XmlElement("ResistRating")]
        public double ResistRating { get; set; }
        
        [XmlElement("StatsExponent")]
        public double StatsExponent { get; set; }

        public Values()
        {
            BaseBlock = 0;
            BaseBlockValue = 0.7;
            BaseCritical = 0.05;
            BaseCriticalValue = 0.05;
            BaseDodge = 0.2;
            BaseEventValue = 0.05;
            BaseHaste = 0;
            BaseHit = 0;
            BaseMiss = 0.15;
            BaseParry = 0.03;
            BaseResistValue = 0.7;
            BigNumberBase = 8;
            CurveExponent = 0.66;
            GstBase = 12;
            GstGoal = 572;
            GstRatio = 5.6;
            ApToDps = 10;
            HpPerEnd = 5;
            LevelCap = 35;
            LevelMax = 85;
            MpPerWis = 5;
            SpToDps = 10;
            ModRating = 3;
            PcDpsMod = 0.85;
            PChpBase1 = 360;
            PChpBase100 = 2000;
            PChpDelta = 1640;
            PChpGoal1 = 400;
            PChpGoal100 = 4000;
            PCmpBase1 = 10;
            PCmpBase100 = 100;
            PCmpDelta = 200;
            PCstBase = 15;
            PCstGoal = 762;
            PCstRatio = 7.47;
            ResistRating = 17;
            StatsExponent = 1;
        }
    }
}