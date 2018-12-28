using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game.Core
{
    [Serializable]
    public class ExpTableData
    {
        [XmlAttribute("Level")]
        public int Level { get; set; }
        [XmlElement("Rate")]
        public int Rate { get; set; }
        [XmlElement("Experience")]
        public int Experience { get; set; }
    }
}