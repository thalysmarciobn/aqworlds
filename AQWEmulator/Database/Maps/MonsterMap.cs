using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class MonsterMap : ClassMap<MonsterModel>
    {
        public MonsterMap()
        {
            Table("hikari_monsters");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Race).Column("Race");
            Map(x => x.File).Column("Data_File");
            Map(x => x.Linkage).Column("Linkage");
            Map(x => x.Element).Column("Element");
            Map(x => x.Level).Column("Level");
            Map(x => x.Heath).Column("Heath");
            Map(x => x.Mana).Column("Mana");
            Map(x => x.Gold).Column("Gold");
            Map(x => x.Coin).Column("Coin");
            Map(x => x.Experience).Column("Experience");
            Map(x => x.FactionId).Column("Faction_ID");
            Map(x => x.Reputation).Column("Reputation");
            Map(x => x.ClassPoint).Column("Class_Point");
            Map(x => x.TeamId).Column("Team_ID");
            Map(x => x.Respawn).Column("Respawn");
            Map(x => x.Dps).Column("Damage");
        }
    }
}