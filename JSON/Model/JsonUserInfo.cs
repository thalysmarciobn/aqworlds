using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonUserInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strFrame { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strPad { get; set; }

        public int uid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonUserData data { get; set; }
    }
}