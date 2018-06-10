using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("mv")]
    public class Move : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (room != user.RoomUser.RoomId) return;
            user.RoomUser.Tx = int.Parse(parameters[0]);
            user.RoomUser.Ty = int.Parse(parameters[1]);

            var speed = int.Parse(parameters[2]);
            NetworkHelper.SendResponseToOthers(
                new[]
                {
                    "xt", "uotls", room.ToString(), user.Name,
                    "tx:" + user.RoomUser.Tx + ",ty:" + user.RoomUser.Ty + ",sp:" + speed + ",strFrame:" +
                    user.RoomUser.Frame
                }, user.RoomUser);
        }
    }
}