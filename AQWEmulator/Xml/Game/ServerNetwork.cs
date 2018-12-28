using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game
{
    [Serializable]
    public class ServerNetwork
    {
        [XmlElement("Host")]
        public string Host { get; set; }
        [XmlElement("Port")]
        public int Port { get; set; }
        [XmlElement("MaxConnections")]
        public int MaxConnections { get; set; }
        [XmlElement("BufferSize")]
        public int BufferSize { get; set; }
        [XmlElement("Backlog")]
        public int Backlog { get; set; }
        [XmlElement("MaxSimultaneousAcceptOps")]
        public int MaxSimultaneousAcceptOps { get; set; }
        [XmlElement("NumOfSaeaForRec")]
        public int NumOfSaeaForRec { get; set; }
        [XmlElement("NumOfSaeaForSend")]
        public int NumOfSaeaForSend  { get; set; }
    }
}