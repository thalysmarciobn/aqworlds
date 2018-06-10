using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World;
using AQWEmulator.World.Core;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("firstJoin")]
    public class FirstJoin : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var firstJoin = new JsonFirstJoin
            {
                o = ServerCoreValues.Core()
            };
            NetworkHelper.SendResponse(new JsonPacket(-1, firstJoin), user);
            RoomsManager.Instance.Join(user, "battleon-1", "Enter", "Spawn", true);
        }
    }
}