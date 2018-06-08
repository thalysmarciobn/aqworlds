using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class PatternMap : ClassMap<PatternModel>
    {
        public PatternMap()
        {
            Table("hikari_patterns");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Description).Column("Description");
            Map(x => x.Wisdom).Column("Wisdom");
            Map(x => x.Strength).Column("Strength");
            Map(x => x.Luck).Column("Luck");
            Map(x => x.Dexterity).Column("Dexterity");
            Map(x => x.Endurance).Column("Endurance");
            Map(x => x.Intelligence).Column("Intelligence");
        }
    }
}