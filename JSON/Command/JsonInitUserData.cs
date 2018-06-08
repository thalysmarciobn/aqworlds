using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonInitUserData : IJsonPacket
    {
        public JsonInitUserData()
        {
            cmd = "initUserData";
        }

        public string cmd { get; }
        public string strFrame { get; set; }
        public string strPad { get; set; }
        public int uid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonUserData data { get; set; }
    }
}