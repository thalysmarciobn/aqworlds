using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonUotls : IJsonPacket
    {
        public JsonUotls()
        {
            cmd = "uotls";
        }

        public string cmd { get; }
        public string unm { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonUoBranch o { get; set; }
    }
}