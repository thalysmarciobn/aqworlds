using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class CharacterItemMap : ClassMap<CharacterItemModel>
    {
        public CharacterItemMap()
        {
            Table("hikari_characters_items");
            Id(x => x.Id).Column("id");
            Map(x => x.UserId).Column("User_ID");
            Map(x => x.CharacterId).Column("Character_ID");
            Map(x => x.ItemId).Column("Item_ID");
            Map(x => x.EnhId).Column("Enhancement_ID");
            Map(x => x.Equipped).Column("Equipped");
            Map(x => x.Quantity).Column("Quantity");
            Map(x => x.Bank).Column("Bank");
            Map(x => x.DatePurchased).Column("Purchased");
            References(x => x.Item, "Item_ID");
            References(x => x.Enhancement, "Enhancement_ID");
        }
    }
}