using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonGarSarsa
    {
        public string cInf { get; set; }
        public string tInf { get; set; }
        public int actID { get; set; }
        public int iRes { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonGarResult> a { get; set; }
    }
}