using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class AuraEffectMap : ClassMap<AuraEffectModel>
    {
        public AuraEffectMap()
        {
            Table("hikari_auras_effects");
            Id(x => x.Id).Column("id");
            Map(x => x.AuraId).Column("Aura_ID");
            Map(x => x.Stat).Column("stat");
            Map(x => x.Value).Column("Value");
            Map(x => x.Type).Column("Type");
        }
    }
}