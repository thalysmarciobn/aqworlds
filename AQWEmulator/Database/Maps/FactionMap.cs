using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class FactionMap : ClassMap<FactionModel>
    {
        public FactionMap()
        {
            Table("hikari_factions");
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
        }
    }
}