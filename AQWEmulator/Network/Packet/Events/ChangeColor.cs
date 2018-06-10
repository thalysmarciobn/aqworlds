using AQWEmulator.Attributes;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using NHibernate.Criterion;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("changeColor")]
    public class ChangeColor : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var skinColor = int.Parse(parameters[0]);
            var hairColor = int.Parse(parameters[1]);
            var eyeColor = int.Parse(parameters[2]);
            var hairId = int.Parse(parameters[3]);
            using (var session = GameFactory.Session.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var hairModel = session.QueryOver<HairModel>().Where(x => x.Id == hairId).SingleOrDefault();
                    if (hairModel == null) return;
                    user.Character.ColorSkin = skinColor.ToString("X").Upper();
                    user.Character.ColorHair = hairColor.ToString("X").Upper();
                    user.Character.ColorEye = eyeColor.ToString("X").Upper();
                    user.Character.HairId = hairId;
                    user.Character.Hair = hairModel;
                    GameFactory.Update(user.Character);
                    transaction.Commit();
                    var changeColor = new JsonChangeColor()
                    {
                        uid = user.Id,
                        HairID = hairId,
                        strHairName = hairModel.Name,
                        strHairFilename = hairModel.File,
                        intColorSkin = skinColor,
                        intColorHair = hairColor,
                        intColorEye = eyeColor
                    };
                    NetworkHelper.SendResponseToOthers(new JsonPacket(room, changeColor), user.RoomUser);
                }
            }
        }
    }
}