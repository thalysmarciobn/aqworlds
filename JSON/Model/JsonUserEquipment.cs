using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonUserEquipment
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ItemID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sFile { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sLink { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sType { get; set; }
    }
}