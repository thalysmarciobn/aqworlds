using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonFirstJoin : IJsonPacket
    {
        public JsonFirstJoin()
        {
            cmd = "cvu";
        }

        public string cmd { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, double> o { get; set; }
    }
}