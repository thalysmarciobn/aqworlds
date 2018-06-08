using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class SkillAuraMap : ClassMap<SkillAuraModel>
    {
        public SkillAuraMap()
        {
            Table("hikari_skills_auras");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Duration).Column("Duration");
            Map(x => x.Category).Column("Category");
            Map(x => x.DamageIncrease).Column("Damage_Increase");
            Map(x => x.DamageTakenDecrease).Column("Damage_Taken_Decrease");
        }
    }
}