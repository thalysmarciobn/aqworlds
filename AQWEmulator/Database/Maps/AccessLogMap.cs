using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class AccessLogMap : ClassMap<AccessLogModel>
    {
        public AccessLogMap()
        {
            Table("logs_characters_access");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();
            Map(x => x.UserId).Column("User_ID");
            Map(x => x.CharId).Column("Character_ID");
            Map(x => x.Address).Column("Address");
            Map(x => x.Date).Column("Date");
        }
    }
}