using System.Collections.Generic;
using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonInitUserDatas : IJsonPacket
    {
        public JsonInitUserDatas()
        {
            cmd = "initUserDatas";
            a = new List<JsonUserInfo>();
        }

        public string cmd { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonUserInfo> a { get; set; }
    }
}