using AQWEmulator.Database.Models;
using FluentNHibernate.Mapping;

namespace AQWEmulator.Database.Maps
{
    public class CharacterMap : ClassMap<CharacterModel>
    {
        public CharacterMap()
        {
            Table("hikari_characters");
            Id(x => x.Id).Column("id");
            Map(x => x.UserId).Column("User_ID");
            Map(x => x.Name).Column("Name");
            Map(x => x.Token).Column("Token");
            Map(x => x.Email).Column("Email");
            Map(x => x.HairId).Column("Hair_ID");
            Map(x => x.Access).Column("Access");
            Map(x => x.ActivationFlag).Column("Activation_Flag");
            Map(x => x.PermamuteFlag).Column("Permamute_Flag");
            Map(x => x.Country).Column("Country");
            Map(x => x.Age).Column("Age");
            Map(x => x.Gender).Column("Gender");
            Map(x => x.Level).Column("Level");
            Map(x => x.Gold).Column("Gold");
            Map(x => x.Coins).Column("Coins");
            Map(x => x.Experience).Column("Experience");
            Map(x => x.ColorHair).Column("Hair_Color");
            Map(x => x.ColorSkin).Column("Skin_Color");
            Map(x => x.ColorEye).Column("Eye_Color");
            Map(x => x.ColorBase).Column("Base_Color");
            Map(x => x.ColorTrim).Column("Trim_Color");
            Map(x => x.ColorAccessory).Column("Accessory_Color");
            Map(x => x.SlotsBag).Column("Bag_Slot");
            Map(x => x.SlotsBank).Column("Bank_Slot");
            Map(x => x.SlotsHouse).Column("House_Slot");
            Map(x => x.DateCreated).Column("created_at");
            Map(x => x.LastAccess).Column("last_access");
            Map(x => x.CpBoostExpire).Column("class_boost");
            Map(x => x.RepBoostExpire).Column("reputation_boost");
            Map(x => x.GoldBoostExpire).Column("gold_boost");
            Map(x => x.ExpBoostExpire).Column("experience_boost");
            Map(x => x.CoinBoostExpire).Column("coin_boost");
            Map(x => x.UpgradeExpire).Column("Upgrade");
            Map(x => x.Upgraded).Column("Upgraded");
            Map(x => x.Achievement).Column("Achievement");
            Map(x => x.Settings).Column("Settings");
            Map(x => x.Quests).Column("Quests");
            Map(x => x.Quests2).Column("Quests_2");
            Map(x => x.DailyQuests0).Column("Daily_Quest_0");
            Map(x => x.DailyQuests1).Column("Daily_Quest_1");
            Map(x => x.DailyQuests2).Column("Daily_Quest_2");
            Map(x => x.MonthlyQuests0).Column("Monthly_Quest_0");
            Map(x => x.LastArea).Column("Last_Area");
            Map(x => x.CurrentServer).Column("Current_Server");
            Map(x => x.HouseInfo).Column("House_Data");
            Map(x => x.Address).Column("Address");
            References(x => x.Hair, "Hair_ID");
            HasMany(x => x.Items).KeyColumn("character_id");
        }
    }
}