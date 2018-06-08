using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonPattern
    {
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sDesc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string iWIS { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string iEND { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string iLCK { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string iSTR { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string iDEX { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string iINT { get; set; }
    }
}