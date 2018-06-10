using System.Collections.Generic;
using AQWEmulator.Attributes;
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
    [PacketIn("loadShop")]
    public class LoadShop : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (!int.TryParse(parameters[0], out var shopId)) return;
            using (var session = GameFactory.Session.OpenSession())
            {
                var shop = session.QueryOver<ShopModel>().Where(x => x.Id == shopId).SingleOrDefault();
                if (shop == null) return;
                IList<JsonItem> items = new List<JsonItem>();
                foreach (var shopItem in shop.Items)
                {
                    // if (!SessionFactory.Cache.Items.TryGetValue(shopItem.ItemId, out var itemModel)) continue;
                    var item = shopItem.Item;
                    var enhancement = item.Enhancement;
                    JsonItem itemJson;
                    if (shop.Staff == 1)
                    {
                        itemJson = Server.Instance.GetItemJson(item);
                        itemJson.ItemID = item.GetHashCode();
                        itemJson.sName = item.Name + " Preview";
                        itemJson.bStaff = 1;
                    }
                    else
                    {
                        itemJson = Server.Instance.GetItemJson(item, enhancement);
                    }

                    if (shop.Limited == 1)
                        itemJson.iQtyRemain = shopItem.QuantityRemain;
                    else
                        itemJson.iQtyRemain = -1;
                    itemJson.ShopItemID = item.Id;
                    itemJson.iReqCP = item.ReqClassPoints;
                    itemJson.iReqRep = item.ReqReputation;
                    itemJson.FactionID = item.FactionId;
                    itemJson.iCost = item.Cost * item.Quantity;
                    if (item.ReqClassId > 0)
                    {
                        //if (SessionFactory.Cache.Classes.TryGetValue(item.ReqClassId, out var classModel))
                        //{
                        //    itemJson.iClass = ((ItemClassModel) classModel).ItemId;
                        //    itemJson.sClass = item.Name;
                        //}

                        itemJson.iClass = 0;
                    }

                    items.Add(itemJson);
                }

                var loadShop = new JsonLoadShop
                {
                    shopinfo = new JsonShopInfo
                    {
                        bHouse = shop.House,
                        bStaff = shop.Staff,
                        bUpgrd = shop.Upgrade,
                        bLimited = shop.Limited,
                        iIndex = -1,
                        ShopID = shop.Id,
                        sField = shop.Field,
                        sName = shop.Name,
                        items = items
                    }
                };
                NetworkHelper.SendResponse(new JsonPacket(-1, loadShop), user);
            }
        }
    }
}