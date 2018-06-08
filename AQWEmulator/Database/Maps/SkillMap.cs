using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class SkillMap : ClassMap<SkillModel>
    {
        public SkillMap()
        {
            Table("hikari_skills");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Animation).Column("Animation");
            Map(x => x.Description).Column("Description");
            Map(x => x.Damage).Column("Damage");
            Map(x => x.Mana).Column("Mana");
            Map(x => x.Icon).Column("Icon");
            Map(x => x.Range).Column("Range");
            Map(x => x.Dsrc).Column("Dsrc");
            Map(x => x.Reference).Column("Reference");
            Map(x => x.Target).Column("Target");
            Map(x => x.Effects).Column("Effects");
            Map(x => x.Type).Column("Type");
            Map(x => x.Strl).Column("Strl");
            Map(x => x.Cooldown).Column("Cooldown");
            Map(x => x.HitTargets).Column("Hit_Targets");
            Map(x => x.AuraId).Column("Aura_ID");
        }
    }
}