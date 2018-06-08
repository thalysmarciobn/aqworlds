using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonUoBranch
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string entType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strFrame { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strPad { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strUsername { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string uoName { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int entID { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intHP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intHPMax { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intMP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intMPMax { get; set; } = -1;

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intLevel { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intState { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int tx { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ty { get; set; } = -1;

        public bool showCloak { get; set; }
        public bool showHelm { get; set; }
        public bool afk { get; set; }
    }
}