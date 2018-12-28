using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game
{
    [Serializable]
    public class ServerSettings
    {
        [XmlElement("ServerId")]
        public int ServerId { get; set; }
        [XmlElement("Rates")]
        public ServerRates ServerRates { get; set; }
        [XmlElement("Database")]
        public ServerDatabase ServerDatabase { get; set; }
        [XmlElement("Network")]
        public ServerNetwork ServerNetwork { get; set; }
        [XmlElement("Limits")]
        public ServerLimits ServerLimits { get; set; }

        public ServerSettings()
        {
            ServerId = 1;
            ServerRates = new ServerRates()
            {
                Gold = 1,
                Coin = 1,
                Experience = 1,
                Class = 1,
                Reputation = 1
            };
            ServerDatabase = new ServerDatabase()
            {
                Host = "127.0.0.1",
                Username = "root",
                Password = "123456",
                Database = "aqworlds"
            };
            ServerNetwork = new ServerNetwork()
            {
                Host = "127.0.0.1",
                Port = 5588,
                MaxConnections = 1000,
                BufferSize = 1024,
                Backlog = 100,
                MaxSimultaneousAcceptOps = 512,
                NumOfSaeaForRec = 320,
                NumOfSaeaForSend = 320
            };
            ServerLimits = new ServerLimits()
            {
                Gold = 4000000,
                Coin = 4000000
            };
        }
    }
}