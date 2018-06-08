using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonUserData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strClassName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strGender { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strHairFilename { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strHairName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strUsername { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dCreated { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string dUpgExp { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string lastArea { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sCountry { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string sHouseInfo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strEmail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strMapName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strQuests { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string strQuests2 { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iCP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iUpgDays { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intAccessLevel { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intColorAccessory { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intColorBase { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intColorEye { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intColorHair { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intColorSkin { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intColorTrim { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intLevel { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intExpUpg { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intRankUpg { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intRankPosition { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CharID { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HairID { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int UserID { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int bPermaMute { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int bitSuccess { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iAge { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iBagSlots { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iBankSlots { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iHouseSlots { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iBoostCP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iBoostG { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iBoostRep { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iBoostXP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iDailyAdCap { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iDailyAds { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iFounder { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int iUpg { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ia0 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ia1 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id0 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id1 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int id2 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int im0 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intActivationFlag { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intCoins { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intExp { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intExpToLevel { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intGold { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intHP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intHPMax { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intMP { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intMPMax { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int intHits { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip0 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip1 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip2 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip3 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip4 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip5 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip6 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip7 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip8 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip9 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip10 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip11 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip12 { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ip13 { get; set; } = -1;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, JsonUserEquipment> eqp { get; set; }
    }
}