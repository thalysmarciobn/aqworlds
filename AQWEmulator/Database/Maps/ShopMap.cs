using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class ShopMap : ClassMap<ShopModel>
    {
        public ShopMap()
        {
            Table("hikari_shops");
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.House).Column("House");
            Map(x => x.Upgrade).Column("Upgrade");
            Map(x => x.Staff).Column("Staff");
            Map(x => x.Limited).Column("Limited");
            Map(x => x.Field).Column("Field");
            HasMany(x => x.Items).KeyColumn("Shop_ID");
        }
    }
}