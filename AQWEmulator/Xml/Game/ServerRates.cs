using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game
{
    [Serializable]
    public class ServerRates
    {
        [XmlElement("Gold")]
        public int Gold { get; set; }
        [XmlElement("Coin")]
        public int Coin { get; set; }
        [XmlElement("Experience")]
        public int Experience { get; set; }
        [XmlElement("Class")]
        public int Class { get; set; }
        [XmlElement("Reputation")]
        public int Reputation { get; set; }
    }
}