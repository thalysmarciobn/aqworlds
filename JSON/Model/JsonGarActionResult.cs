using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonGarActionResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cInf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string tInf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int hp { get; set; } = -1;
    }
}