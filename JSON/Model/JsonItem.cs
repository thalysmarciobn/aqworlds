using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonItem
    {
        public string sDesc { get; set; }
        public string sES { get; set; }
        public string sElmt { get; set; }
        public string sFile { get; set; }
        public string sIcon { get; set; }
        public string sLink { get; set; }
        public string sMeta { get; set; }
        public string sName { get; set; }
        public string sReqQuests { get; set; }
        public string sType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dPurchase { get; set; }

        public int ItemID { get; set; }
        public int bCoins { get; set; }
        public int bHouse { get; set; }
        public int bPTR { get; set; }
        public int bStaff { get; set; }
        public int bTemp { get; set; }
        public int bUpg { get; set; }
        public int iCost { get; set; }
        public int iDPS { get; set; }
        public int iLvl { get; set; }
        public int iQSindex { get; set; }
        public int iQSvalue { get; set; }
        public int iRng { get; set; }
        public int iRty { get; set; }
        public int iStk { get; set; }
        public int PatternID { get; set; }
        public int EnhID { get; set; }
        public int EnhLvl { get; set; }
        public int EnhPatternID { get; set; }
        public int EnhRty { get; set; }
        public int EnhRng { get; set; }
        public int InvEnhPatternID { get; set; }
        public int EnhDPS { get; set; }

        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string sClass { get; set; } = "";

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int bBank { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CharItemID { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iQty { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iHrs { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int bEquip { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ShopItemID { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iReqCP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iReqRep { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int FactionID { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iClass { get; set; } = -1;

        [DefaultValue(-2)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iQtyRemain { get; set; } = -2;
    }
}