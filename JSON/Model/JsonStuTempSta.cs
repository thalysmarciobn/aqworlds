using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonStuTempSta
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, int> ba { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, int> ar { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, int> he { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, int> Weapon { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonStuTempStaInnate innate { get; set; }
    }
}