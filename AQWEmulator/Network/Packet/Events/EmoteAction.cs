using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("emotea")]
    public class EmoteAction : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            NetworkHelper.SendResponseToOthers(new[] {"emotea", parameters[0], user.Id.ToString()}, user.RoomUser);
        }
    }
}