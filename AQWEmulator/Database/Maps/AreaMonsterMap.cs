using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class AreaMonsterMap : ClassMap<AreaMonsterModel>
    {
        public AreaMonsterMap()
        {
            Table("hikari_areas_monsters");
            Map(x => x.MonMapId).Column("Monster_Map_ID");
            Map(x => x.AreaId).Column("Area_ID");
            Map(x => x.MonId).Column("Monster_ID");
            Map(x => x.Frame).Column("Frame");
            References(x => x.Monster, "Monster_ID");
        }
    }
}