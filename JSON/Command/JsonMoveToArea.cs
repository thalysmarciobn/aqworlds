using System.Collections.Generic;
using JSON.Model;
using Newtonsoft.Json;

namespace JSON.Command
{
    public sealed class JsonMoveToArea : IJsonPacket
    {
        public JsonMoveToArea()
        {
            cmd = "moveToArea";
        }

        public string cmd { get; }
        public string areaName { get; set; }
        public string sExtra { get; set; }
        public string strMapFileName { get; set; }
        public string strMapName { get; set; }
        public int intType { get; set; }
        public int areaId { get; set; }
        public IList<JsonUoBranch> uoBranch { get; set; }
        public IList<JsonMonBranch> monBranch { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonMonsterDefinition> mondef { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IList<JsonMonMap> monmap { get; set; }
    }
}