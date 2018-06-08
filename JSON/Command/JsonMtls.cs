using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonMtls : IJsonPacket
    {
        public JsonMtls()
        {
            cmd = "mtls";
        }

        public string cmd { get; }
        public int id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonState o { get; set; }
    }
}