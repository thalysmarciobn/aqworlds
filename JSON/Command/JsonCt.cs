using System.Collections.Generic;
using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonCt : IJsonPacket
    {
        public JsonCt()
        {
            cmd = "ct";
        }

        public string cmd { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JsonState> p { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JsonState> m { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<int> a { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonGarSarsa> sarsa { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonGarSara> sara { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonGarAnims> anims { get; set; }
    }
}