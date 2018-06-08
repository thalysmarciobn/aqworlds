using System;
using System.Collections.Generic;

namespace AQWEmulator.Database.Models
{
    public class CharacterModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int UserId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Token { get; set; }
        public virtual string Email { get; protected set; }
        public virtual int HairId { get; set; }
        public virtual int Access { get; set; }
        public virtual int ActivationFlag { get; set; }
        public virtual int PermamuteFlag { get; set; }
        public virtual string Country { get; set; }
        public virtual int Age { get; set; }
        public virtual string Gender { get; set; }
        public virtual int Level { get; set; }
        public virtual int Gold { get; set; }
        public virtual int Coins { get; set; }
        public virtual int Experience { get; set; }
        public virtual string ColorHair { get; set; }
        public virtual string ColorSkin { get; set; }
        public virtual string ColorEye { get; set; }
        public virtual string ColorBase { get; set; }
        public virtual string ColorTrim { get; set; }
        public virtual string ColorAccessory { get; set; }
        public virtual int SlotsBag { get; set; }
        public virtual int SlotsBank { get; set; }
        public virtual int SlotsHouse { get; set; }
        public virtual DateTime DateCreated { get; protected set; }
        public virtual DateTime LastAccess { get; set; }
        public virtual DateTime CpBoostExpire { get; set; }
        public virtual DateTime RepBoostExpire { get; set; }
        public virtual DateTime GoldBoostExpire { get; set; }
        public virtual DateTime ExpBoostExpire { get; set; }
        public virtual DateTime CoinBoostExpire { get; set; }
        public virtual DateTime UpgradeExpire { get; set; }
        public virtual int Upgraded { get; set; }
        public virtual int Achievement { get; set; }
        public virtual int Settings { get; set; }
        public virtual string Quests { get; set; }
        public virtual string Quests2 { get; set; }
        public virtual int DailyQuests0 { get; set; }
        public virtual int DailyQuests1 { get; set; }
        public virtual int DailyQuests2 { get; set; }
        public virtual int MonthlyQuests0 { get; set; }
        public virtual string LastArea { get; set; }
        public virtual string CurrentServer { get; set; }
        public virtual string HouseInfo { get; set; }
        public virtual string Address { get; set; }
        public virtual HairModel Hair { get; set; }
        public virtual IList<CharacterItemModel> Items { get; set; }
    }
}