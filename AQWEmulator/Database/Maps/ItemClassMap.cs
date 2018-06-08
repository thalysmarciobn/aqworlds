using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class ItemClassMap : ClassMap<ItemClassModel>
    {
        public ItemClassMap()
        {
            Table("hikari_classes");
            Id(x => x.ItemId).Column("Item_ID");
            Map(x => x.Category).Column("Category");
            Map(x => x.Description).Column("Description");
            Map(x => x.ManaRegenerationMethods).Column("Mana_Regeneration_Methods");
            Map(x => x.StatsDescription).Column("Stats_Description");
            HasMany(x => x.Skills).KeyColumn("Item_ID");
        }
    }
}