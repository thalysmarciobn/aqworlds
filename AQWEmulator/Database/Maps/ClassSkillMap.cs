using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class ClassSkillMap : ClassMap<ClassSkillModel>
    {
        public ClassSkillMap()
        {
            Table("hikari_classes_skills");
            Id(x => x.Id).Column("id");
            Map(x => x.ItemId).Column("Item_ID");
            Map(x => x.SkillId).Column("Skill_ID");
            HasOne(x => x.Skill).ForeignKey("Skill_ID");
        }
    }
}