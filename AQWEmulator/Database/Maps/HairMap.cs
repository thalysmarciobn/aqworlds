using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    internal class HairMap : ClassMap<HairModel>
    {
        public HairMap()
        {
            Table("hikari_hairs");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.File).Column("Data_File");
            Map(x => x.Name).Column("Name");
            Map(x => x.Gender).Column("Gender");
        }
    }
}