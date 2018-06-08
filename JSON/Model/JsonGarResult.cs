using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonGarResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cInf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string tInf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string typ { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int hp { get; set; } = -1;
    }
}