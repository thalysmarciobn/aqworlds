using AQWEmulator.Attributes;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("mtcid")]
    public class MoveToCellById : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
        }
    }
}