using System.Net;

namespace AQWEmulator.Settings
{
    public class NetworkSettings
    {
        public IPEndPoint Endpoint { get; set; }
        public int MaxConnections { get; set; }
        public int BufferSize { get; set; }
        public int Backlog { get; set; }
        public int MaxSimultaneousAcceptOps { get; set; }
        public int NumOfSaeaForRec { get; set; }
        public int NumOfSaeaForSend  { get; set; }
    }
}