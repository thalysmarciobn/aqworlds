using System;
using System.Globalization;
using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("retrieveUserData")]
    public class RetrieveUserData : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (!int.TryParse(parameters[0], out var playerId)) return;
            var client = UsersManager.Instance.GetUserById(playerId);
            if (client == null) return;
            var character = client.Character;
            var hairModel = character.Hair;
            var itemModelClass = client.UserClass.ItemModel;
            var charcaterClassItemModel = client.UserClass.CharacterItem;
            var data = new JsonInitUserData
            {
                strFrame = client.RoomUser.Frame,
                strPad = client.RoomUser.Pad,
                uid = client.Id,
                data = new JsonUserData
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
                }
            };
            NetworkHelper.SendResponse(new JsonPacket(room, data), user);
        }
    }
}