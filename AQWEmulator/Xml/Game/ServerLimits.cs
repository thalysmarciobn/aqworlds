using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game
{
    [Serializable]
    public class ServerLimits
    {
        [XmlElement("Gold")]
        public int Gold { get; set; }
        [XmlElement("Coin")]
        public int Coin { get; set; }
    }
}