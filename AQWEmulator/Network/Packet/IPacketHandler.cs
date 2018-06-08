using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet
{
    public interface IPacketHandler
    {
        void Dispatch(User user, int room, string[] parameters);
    }
}