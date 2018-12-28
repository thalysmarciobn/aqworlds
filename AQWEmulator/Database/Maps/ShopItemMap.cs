using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class ShopItemMap : ClassMap<ShopItemModel>
    {
        public ShopItemMap()
        {
            Table("hikari_shops_items");
            Id(x => x.Id).Column("id");
            Map(x => x.ShopId).Column("Shop_ID");
            Map(x => x.ItemId).Column("Item_ID");
            Map(x => x.QuantityRemain).Column("Quantity_Remain");
            References(x => x.Item, "Item_ID");
        }
    }
}