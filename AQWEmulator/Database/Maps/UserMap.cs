using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class UserMap : ClassMap<UserModel>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Email).Column("Email");
            Map(x => x.LastAccess).Column("Last_Access");
        }
    }
}