using System.Collections.Generic;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonParams
    {
        public string Cmd { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Params { get; set; }
    }
}