using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("trap door")]
    public class TrapDoor : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            NetworkHelper.SendResponse(new[] {"xt", "trap door", parameters[0]}, user.RoomUser);
        }
    }
}