using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class EnhancementMap : ClassMap<EnhancementModel>
    {
        public EnhancementMap()
        {
            Table("hikari_enhancements");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.PatternId).Column("Pattern_ID");
            Map(x => x.Rarity).Column("Rarity");
            Map(x => x.Dps).Column("DPS");
            Map(x => x.Level).Column("Level");
            References(x => x.Pattern, "Pattern_ID");
        }
    }
}