using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("isModerator")]
    public class IsModerator : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (room != user.RoomUser.RoomId) return;
            var target = parameters[0].ToLower();
            if (!UsersManager.Instance.ContainsUserByName(target)) return;
            var client = UsersManager.Instance.GetUserByName(target);
            NetworkHelper.SendResponse(
                new JsonPacket(-1, new JsonIsModerator {val = client.Character.Access >= 30, unm = client.Name}),
                user);
        }
    }
}