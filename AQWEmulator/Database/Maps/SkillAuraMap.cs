using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class SkillAuraMap : ClassMap<SkillAuraModel>
    {
        public SkillAuraMap()
        {
            Table("hikari_skills_auras");
            Id(x => x.Id).Column("id");
            Map(x => x.SkillId).Column("Skill_ID");
            Map(x => x.AuraId).Column("Aura_ID");
            HasOne(x => x.Aura).ForeignKey("Aura_ID");
        }
    }
}