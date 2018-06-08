using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonAddGoldExp : IJsonPacket
    {
        public JsonAddGoldExp()
        {
            cmd = "addGoldExp";
        }

        public int id { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intGold { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intCoin { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intExp { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iCP { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FactionID { get; set; }

        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iRep { get; set; }

        public string cmd { get; }
        public string typ { get; set; }
    }
}