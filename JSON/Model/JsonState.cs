using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonState
    {
        public int intHP { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intHPMax { get; set; } = -1;

        public int intMP { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intMPMax { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intState { get; set; } = -1;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int[] targets { get; set; }
    }
}