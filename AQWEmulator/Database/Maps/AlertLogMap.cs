using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class AlertLogMap : ClassMap<AlertLogModel>
    {
        public AlertLogMap()
        {
            Table("logs_characters_alerts");
            Id(x => x.Id).Column("id").GeneratedBy.Increment();
            Map(x => x.UserId).Column("User_ID");
            Map(x => x.CharId).Column("Character_ID");
            Map(x => x.Reason).Column("Reason");
            Map(x => x.Address).Column("Address");
            Map(x => x.Date).Column("Date");
        }
    }
}