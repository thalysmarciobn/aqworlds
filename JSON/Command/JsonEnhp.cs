using System.Collections.Generic;
using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonEnhp : IJsonPacket
    {
        public JsonEnhp()
        {
            cmd = "enhp";
        }

        public string cmd { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JsonPattern> o { get; set; }
    }
}