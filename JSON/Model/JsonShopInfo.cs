using System.Collections.Generic;
using System.ComponentModel;

namespace JSON.Model
{
    public sealed class JsonShopInfo
    {
        [DefaultValue(-1)] public string sField { get; set; } = "";

        public string sName { get; set; }

        public int bHouse { get; set; }

        public int bStaff { get; set; }

        public int bUpgrd { get; set; }

        public int bLimited { get; set; }

        public int iIndex { get; set; }

        public int ShopID { get; set; }

        public IList<JsonItem> items { get; set; }
    }
}