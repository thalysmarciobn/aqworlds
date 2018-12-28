using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class ItemMap : ClassMap<ItemModel>
    {
        public ItemMap()
        {
            Table("hikari_items");
            Id(x => x.Id).Column("id");
            Map(x => x.Name).Column("Name");
            Map(x => x.Description).Column("Description");
            Map(x => x.Type).Column("Type");
            Map(x => x.Element).Column("Element");
            Map(x => x.File).Column("Data_File");
            Map(x => x.Link).Column("Link");
            Map(x => x.Icon).Column("Icon");
            Map(x => x.Equipment).Column("Equipment");
            Map(x => x.Level).Column("Level");
            Map(x => x.Dps).Column("DPS");
            Map(x => x.Range).Column("Item_Range");
            Map(x => x.Rarity).Column("Rarity");
            Map(x => x.Quantity).Column("Quantity");
            Map(x => x.Stack).Column("Stack");
            Map(x => x.Cost).Column("Cost");
            Map(x => x.Coins).Column("Coins");
            Map(x => x.Sellable).Column("Sellable");
            Map(x => x.Temporary).Column("Temporary");
            Map(x => x.Upgrade).Column("Upgrade");
            Map(x => x.Staff).Column("Staff");
            Map(x => x.EnhId).Column("Enhancement_ID");
            Map(x => x.FactionId).Column("Faction_ID");
            Map(x => x.ReqReputation).Column("Required_Reputation");
            Map(x => x.ReqClassId).Column("Required_Class_ID");
            Map(x => x.ReqClassPoints).Column("Required_Class_Points");
            Map(x => x.ReqQuests).Column("Required_Quests");
            Map(x => x.QuestStringIndex).Column("Quest_String_Index");
            Map(x => x.QuestStringValue).Column("Quest_String_Value");
            Map(x => x.Meta).Column("Meta");
            References(x => x.Enhancement, "Enhancement_ID");
            HasOne(x => x.ItemClass);
        }
    }
}