using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("cc")]
    public class CannedChat : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            NetworkHelper.SendResponse(new[] {"xt", "cc", "-1", parameters[0], user.Name}, user);
        }
    }
}