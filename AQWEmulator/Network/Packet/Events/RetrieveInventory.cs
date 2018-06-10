using System;
using System.Collections.Generic;
using System.Linq;
using AQWEmulator.Attributes;
using AQWEmulator.Combat;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.World;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("retrieveInventory")]
    internal class RetrieveInventory : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            IList<object> factions = new List<object>();
            IList<JsonItem> houseItems = new List<JsonItem>();
            IList<JsonItem> inventoryItems = new List<JsonItem>();
            var eqp = new Dictionary<string, JsonUserEquipment>();
            var userCharacterItems = user.Character.Items;
            foreach (var userItemModel in userCharacterItems)
            {
                var itemModel = userItemModel.Item;
                if (itemModel == null) return;
                var enhancementModel = userItemModel.Enhancement;
                if (enhancementModel == null) return;
                var itemJson = Server.Instance.GetItemJson(itemModel, enhancementModel);
                itemJson.bBank = userItemModel.Bank;
                itemJson.CharItemID = userItemModel.Id;
                itemJson.iQty = userItemModel.Quantity;
                itemJson.bEquip = userItemModel.Equipped;
                if (itemModel.Coins == 1)
                {
                    itemJson.iHrs = Convert.ToInt32((DateTime.Now - userItemModel.DatePurchased).TotalHours);
                    itemJson.dPurchase = userItemModel.DatePurchased.ToString("yyyy-MM-dd'T'HH:mm:ss");
                }

                if (userItemModel.Equipped == 1)
                    if (!eqp.ContainsKey(itemModel.Equipment))
                    {
                        eqp.Add(itemModel.Equipment, new JsonUserEquipment
                        {
                            ItemID = itemModel.Id.ToString(),
                            sFile = itemModel.File,
                            sLink = itemModel.Link,
                            sType = itemModel.Type
                        });
                        var equipItem = new JsonEquipItem
                        {
                            uid = user.Id,
                            ItemID = itemModel.Id,
                            strES = itemModel.Equipment,
                            sFile = itemModel.File,
                            sLink = itemModel.Link,
                            sMeta = itemModel.Meta,
                            sType = itemModel.Type
                        };
                        NetworkHelper.SendResponse(new JsonPacket(-1, equipItem), user.RoomUser);
                        if (itemModel.Equipment.Equals(ItemType.Class) || itemModel.Equipment.Equals(ItemType.Cape) ||
                            itemModel.Equipment.Equals(ItemType.Helm) || itemModel.Equipment.Equals(ItemType.Weapon))
                        {
                            if (itemModel.Equipment.Equals(ItemType.Weapon))
                            {
                                user.UserStats.WeaponModel = itemModel;
                                user.UserStats.WeaponEnhancementModel = enhancementModel;
                            }
                            else if (itemModel.Equipment.Equals(ItemType.Class))
                            {
                                if (itemModel.ItemClass != null)
                                {
                                    user.UserClass.CharacterItem = userItemModel;
                                    user.UserClass.ItemModel = itemModel;
                                    JsonHelper.UpdateClass(user);
                                    JsonHelper.LoadSkills(user);
                                    Server.Instance.UpdateStats(user, userCharacterItems);
                                }
                            }
                        }
                    }

                if (userItemModel.Bank == 0)
                    if (!itemModel.Equipment.Equals("ho") && !itemModel.Equipment.Equals("hi"))
                        inventoryItems.Add(itemJson);
                    else
                        houseItems.Add(itemJson);
            }

            user.Equipment = eqp;

            using (var session = GameFactory.Session.OpenSession())
            {
                var enhp = new JsonEnhp
                {
                    o = session.QueryOver<PatternModel>().List<PatternModel>()
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
                        })
                };
                NetworkHelper.SendResponse(new JsonPacket(-1, enhp), user);
            }

            var inventory = new JsonLoadInventoryBig
            {
                bankCount = 0,
                items = inventoryItems,
                hitems = houseItems,
                factions = factions
            };
            NetworkHelper.SendResponse(new JsonPacket(-1, inventory), user);
            JsonHelper.SendStats(user, false);
        }
    }
}