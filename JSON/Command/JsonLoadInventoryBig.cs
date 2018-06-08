using System.Collections.Generic;
using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonLoadInventoryBig : IJsonPacket
    {
        public JsonLoadInventoryBig()
        {
            cmd = "loadInventoryBig";
        }

        public string cmd { get; }
        public int bankCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonItem> items { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonItem> hitems { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<object> factions { get; set; }
    }
}