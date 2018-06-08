using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class ServerMap : ClassMap<ServerModel>
    {
        public ServerMap()
        {
            Table("hikari_servers");
            Not.LazyLoad();
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Upgrade).Column("Upgrade");
            Map(x => x.Online).Column("Online");
            Map(x => x.Chat).Column("Chat");
            Map(x => x.Max).Column("Max");
            Map(x => x.Motd).Column("MOTD");
            Map(x => x.Count).Column("Count");
        }
    }
}