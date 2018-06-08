using System.ComponentModel;
using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonStu : IJsonPacket
    {
        public JsonStu()
        {
            cmd = "stu";
        }

        public string cmd { get; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int wDPS { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int mDPS { get; set; } = -1;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonStuSta sta { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JsonStuTempSta tempSta { get; set; }
    }
}