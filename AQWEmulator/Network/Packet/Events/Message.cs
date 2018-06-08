using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("message")]
    public class Message : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (room != user.RoomUser.RoomId) return;
            var channel = parameters[1];
            var message = parameters[0];
            if (message.Length > 150) message = message.Substring(0, 150);
            switch (channel)
            {
                default:
                    NetworkHelper.SendResponse(
                        new[] {"xt", "chatm", room.ToString(), "zone~" + message, user.Name, room.ToString()},
                        user.RoomUser);
                    break;
            }
        }
    }
}