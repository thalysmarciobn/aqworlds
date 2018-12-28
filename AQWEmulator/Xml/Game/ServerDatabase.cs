using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game
{
    [Serializable]
    public class ServerDatabase
    {
        [XmlElement("Host")]
        public string Host { get; set; }
        [XmlElement("Username")]
        public string Username { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("Database")]
        public string Database { get; set; }
    }
}