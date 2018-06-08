using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonGarSara
    {
        public int iRes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonGarActionResult actionResult { get; set; }
    }
}