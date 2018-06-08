using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Database;
using Database.Models;
using JSON.Command;
using JSON.Model;
using World;
using World.Core;
using World.Helper;
using World.Packet;
using World.Users;

namespace Extension.Packet.Events
{
    public class LoadUser : IPacketEvent
    {
        public void Dispatch(User user, string[] parameters)
        {
            IList<object> factions = new List<object>();
            IList<JsonItem> houseItems = new List<JsonItem>();
            IList<JsonItem> inventoryItems = new List<JsonItem>();
            var eqp = new Dictionary<string, JsonUserEquipment>();
            using (var session = SessionFactory.Session.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var userItemModel in session.QueryOver<CharacterItemModel>()
                        .Where(x => x.CharacterId == user.Character.Id).List<CharacterItemModel>())
                        if (SessionFactory.Cache.Items.TryGetValue(userItemModel.ItemId, out var itemModel))
                        {
                            var item = (ItemModel) itemModel;
                            var enhancementModel =
                                (EnhancementModel) SessionFactory.Cache.Enhancements[userItemModel.EnhId];
                            var itemJson = Server.GetItemJson(item, enhancementModel);
                            itemJson.bBank = userItemModel.Bank;
                            itemJson.CharItemID = userItemModel.Id;
                            itemJson.iQty = userItemModel.Quantity;
                            itemJson.bEquip = userItemModel.Equipped;
                            if (item.Coins == 1)
                            {
                                itemJson.iHrs =
                                    Convert.ToInt32((DateTime.Now - userItemModel.DatePurchased).TotalHours);
                                itemJson.dPurchase = userItemModel.DatePurchased.ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            if (userItemModel.Equipped == 1)
                                if (!eqp.ContainsKey(item.Equipment))
                                {
                                    eqp.Add(item.Equipment, new JsonUserEquipment
                                    {
                                        ItemID = item.Id.ToString(),
                                        sFile = item.File,
                                        sLink = item.Link,
                                        sType = item.Type
                                    });
                                    var equipItem = new JsonEquipItem
                                    {
                                        uid = user.Id,
                                        ItemID = item.Id,
                                        strES = item.Equipment,
                                        sFile = item.File,
                                        sLink = item.Link,
                                        sMeta = item.Meta,
                                        sType = item.Type
                                    };
                                    //NetworkHelper.SendResponse(new JsonPacket(-1, equipItem), user.RoomUser);
                                    if (item.Equipment.Equals("ar") || item.Equipment.Equals("ba") ||
                                        item.Equipment.Equals("he") || item.Equipment.Equals("Weapon"))
                                    {
                                        if (item.Equipment.Equals("Weapon"))
                                        {
                                            user.UserStats.WeaponModel = item;
                                            user.UserStats.WeaponEnhancementModel = enhancementModel;
                                        }
                                        else if (item.Equipment.Equals("ar"))
                                        {
                                            if (SessionFactory.Cache.Classes.TryGetValue(item.Id,
                                                out var classModel))
                                            {
                                                user.UserClass.CharacterItem = userItemModel;
                                                user.UserClass.ItemModel = item;
                                                user.UserClass.ClassModel = (ItemClassModel) classModel;
                                                JsonHelper.UpdateClass(user);
                                                JsonHelper.LoadSkills(user);
                                            }
                                        }

                                        if (SessionFactory.Cache.Patterns.TryGetValue(enhancementModel.PatternId,
                                            out var patternModel))
                                            Server.UpdateStats(user, enhancementModel, (PatternModel) patternModel,
                                                item.Equipment);
                                    }
                                }

                            if (userItemModel.Bank == 0)
                                if (!item.Equipment.Equals("ho") && !item.Equipment.Equals("hi"))
                                    inventoryItems.Add(itemJson);
                                else
                                    houseItems.Add(itemJson);
                        }

                    user.Equipment = eqp;
                    var enhp = new JsonEnhp();
                    var o = SessionFactory.Cache.Patterns.Values.Cast<PatternModel>()
                        .ToDictionary(pattern => pattern.Id.ToString(), pattern => new JsonPattern
                        {
                            ID = pattern.Id,
                            sName = pattern.Name,
                            sDesc = pattern.Description,
                            iWIS = pattern.Wisdom.ToString(),
                            iEND = pattern.Endurance.ToString(),
                            iLCK = pattern.Luck.ToString(),
                            iSTR = pattern.Strength.ToString(),
                            iDEX = pattern.Dexterity.ToString(),
                            iINT = pattern.Intelligence.ToString()
                        });
                    enhp.o = o;
                    //NetworkHelper.SendResponse(new JsonPacket(-1, enhp), user);
                    var inventory = new JsonLoadInventoryBig
                    {
                        bankCount = 0,
                        items = inventoryItems,
                        hitems = houseItems,
                        factions = factions
                    };
                    //NetworkHelper.SendResponse(new JsonPacket(-1, inventory), user);
                    JsonHelper.SendStats(user, false);
                    transaction.Commit();
                }
            }
            var state = user.UserState;
            var character = user.Character;
            Console.WriteLine("asasassa");
            if (SessionFactory.Cache.Hairs.TryGetValue(character.HairId, out var model))
            {
                var hairModel = (HairModel) model;

                var itemModelClass = user.UserClass.ItemModel;
                var charcaterClassItemModel = user.UserClass.CharacterItem;
                var coreExp = ServerCoreValues.GetExpToLevel(character.Level);
                var userData = new JsonUserData
                {
                    eqp = user.Equipment,
                    strClassName = itemModelClass != null ? itemModelClass.Name : "",
                    strGender = character.Gender,
                    strHairFilename = hairModel.File,
                    strHairName = hairModel.Name,
                    strUsername = character.Name,
                    iCP = charcaterClassItemModel?.Quantity ?? 0,
                    iUpgDays = (character.UpgradeExpire - DateTime.Now).Days,
                    intAccessLevel = character.Access,
                    intColorAccessory = int.Parse(character.ColorAccessory, NumberStyles.HexNumber),
                    intColorBase = int.Parse(character.ColorBase, NumberStyles.HexNumber),
                    intColorEye = int.Parse(character.ColorEye, NumberStyles.HexNumber),
                    intColorHair = int.Parse(character.ColorHair, NumberStyles.HexNumber),
                    intColorSkin = int.Parse(character.ColorSkin, NumberStyles.HexNumber),
                    intColorTrim = int.Parse(character.ColorTrim, NumberStyles.HexNumber),
                    intLevel = character.Level,
                    intExpUpg = 0,
                    intRankUpg = 0,
                    intRankPosition = 9999999,
                    CharID = character.Id,
                    HairID = character.HairId,
                    UserID = character.UserId,
                    bPermaMute = character.PermamuteFlag,
                    bitSuccess = 1,
                    dCreated = character.DateCreated.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                    dUpgExp = character.UpgradeExpire.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                    iAge = character.Age,
                    iBagSlots = character.SlotsBag,
                    iBankSlots = character.SlotsBank,
                    iHouseSlots = character.SlotsHouse,
                    iBoostCP = 0,
                    iBoostG = 0,
                    iBoostRep = 0,
                    iBoostXP = 0,
                    iDailyAdCap = 6,
                    iDailyAds = 0,
                    iFounder = 0,
                    iUpg = character.Upgraded,
                    ia0 = 0,
                    ia1 = 0,
                    id0 = 0,
                    id1 = 0,
                    id2 = 0,
                    im0 = 0,
                    intActivationFlag = character.ActivationFlag,
                    intCoins = character.Coins,
                    intExp = character.Experience,
                    intExpToLevel = coreExp?.Experience ?? 20000000,
                    intGold = character.Gold,
                    intHP = state.Health,
                    intHPMax = state.MaxHealth,
                    intMP = state.Mana,
                    intMPMax = state.MaxMana,
                    intHits = 1267,
                    ip0 = 0,
                    ip1 = 0,
                    ip2 = 0,
                    ip3 = 0,
                    ip4 = 0,
                    ip5 = 0,
                    ip6 = 0,
                    ip7 = 0,
                    ip8 = 0,
                    ip9 = 0,
                    ip10 = 0,
                    ip11 = 0,
                    ip12 = 0,
                    ip13 = 0,
                    lastArea = user.RoomUser.RoomName,
                    sCountry = character.Country,
                    sHouseInfo = character.HouseInfo,
                    strEmail = character.Email,
                    strMapName = user.RoomUser.RoomName,
                    strQuests = character.Quests,
                    strQuests2 = character.Quests2
                };
                NetworkHelper.SendResponse(new JsonInitPlayer()
                {
                    bankCount = 0,
                    equippedHouse = -1,
                    items = inventoryItems,
                    factions = factions,
                    playerInfo = userData,
                }, user);
            }
        }
    }
}