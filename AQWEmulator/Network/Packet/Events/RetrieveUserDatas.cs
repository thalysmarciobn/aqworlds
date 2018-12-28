using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World;
using AQWEmulator.World.Core;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("retrieveUserDatas")]
    public class RetrieveUserDatas : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            IList<JsonUserInfo> users = new List<JsonUserInfo>();
            var datas = new JsonInitUserDatas();
            foreach (var param in parameters)
            {
                var client = UsersManager.Instance.GetUserById(int.Parse(param));
                if (client != null)
                {
                    var state = client.UserState;
                    var character = client.Character;
                    var hairModel = character.Hair;

                    var itemModelClass = client.UserClass.ItemModel;
                    var charcaterClassItemModel = client.UserClass.CharacterItem;
                    var userData = new JsonUserData
                    {
                        eqp = client.Equipment,
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
                        intRankPosition = 9999999
                    };
                    if (user.Id == client.Id)
                    {
                        userData.CharID = character.Id;
                        userData.HairID = character.HairId;
                        userData.UserID = character.UserId;
                        userData.bPermaMute = character.PermamuteFlag;
                        userData.bitSuccess = 1;
                        userData.dCreated = character.DateCreated.ToString("yyyy-MM-dd'T'HH:mm:ss");
                        userData.dUpgExp = character.UpgradeExpire.ToString("yyyy-MM-dd'T'HH:mm:ss");
                        userData.iAge = character.Age;
                        userData.iBagSlots = character.SlotsBag;
                        userData.iBankSlots = character.SlotsBank;
                        userData.iHouseSlots = character.SlotsHouse;
                        userData.iBoostCP = 0;
                        userData.iBoostG = 0;
                        userData.iBoostRep = 0;
                        userData.iBoostXP = 0;
                        userData.iDailyAdCap = 6;
                        userData.iDailyAds = 0;
                        userData.iFounder = 0;
                        userData.iUpg = character.Upgraded;
                        userData.ia0 = 0;
                        userData.ia1 = 0;
                        userData.id0 = 0;
                        userData.id1 = 0;
                        userData.id2 = 0;
                        userData.im0 = 0;
                        userData.intActivationFlag = character.ActivationFlag;
                        userData.intCoins = character.Coins;
                        userData.intExp = character.Experience;
                        var coreExp = ServerCoreValues.ExpTable.Configuration.Levels.FirstOrDefault(x => x.Level == character.Level);
                        userData.intExpToLevel = coreExp?.Experience ?? 20000000;
                        userData.intGold = character.Gold;
                        userData.intHP = state.Health;
                        userData.intHPMax = state.MaxHealth;
                        userData.intMP = state.Mana;
                        userData.intMPMax = state.MaxMana;
                        userData.intHits = 1267;
                        userData.ip0 = 0;
                        userData.ip1 = 0;
                        userData.ip2 = 0;
                        userData.ip3 = 0;
                        userData.ip4 = 0;
                        userData.ip5 = 0;
                        userData.ip6 = 0;
                        userData.ip7 = 0;
                        userData.ip8 = 0;
                        userData.ip9 = 0;
                        userData.ip10 = 0;
                        userData.ip11 = 0;
                        userData.ip12 = 0;
                        userData.ip13 = 0;
                        userData.lastArea = client.RoomUser.RoomName;
                        userData.sCountry = character.Country;
                        userData.sHouseInfo = character.HouseInfo;
                        userData.strEmail = character.Email;
                        userData.strMapName = client.RoomUser.RoomName;
                        userData.strQuests = character.Quests;
                        userData.strQuests2 = character.Quests2;
                    }

                    users.Add(new JsonUserInfo
                    {
                        uid = client.Id,
                        strFrame = client.RoomUser.Frame,
                        strPad = client.RoomUser.Pad,
                        data = userData
                    });
                }

                datas.a = users;
            }

            NetworkHelper.SendResponse(new JsonPacket(room, datas), user);
        }
    }
}