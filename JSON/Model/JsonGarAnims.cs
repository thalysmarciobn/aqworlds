using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonGarAnims
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strFrame { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string fx { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string tInf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cInf { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string animStr { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strl { get; set; }
    }
}