using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class AreaMap : ClassMap<AreaModel>
    {
        public AreaMap()
        {
            Table("hikari_areas");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.File).Column("Area_File");
            Map(x => x.MaxPlayers).Column("Players_Max");
            Map(x => x.ReqLevel).Column("Required_Level");
            Map(x => x.Upgrade).Column("Upgrade");
            Map(x => x.Staff).Column("Staff");
            Map(x => x.Pvp).Column("PVP");
            Map(x => x.Pk).Column("PK");
            HasMany(x => x.AreaMonster).KeyColumn("Area_ID");
        }
    }
}