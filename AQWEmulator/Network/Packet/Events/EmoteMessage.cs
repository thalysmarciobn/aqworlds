using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("em")]
    public class EmoteMessage : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (user.Character.PermamuteFlag > 0)
            {
                NetworkHelper.SendResponse(new[] {"xt", "warning", "-1", "You are muted! Chat privileges have been permanently revoked."}, user);
                return;
            }
            NetworkHelper.SendResponse(new[] {"xt", "em", user.Name, parameters[0]}, user);
        }
    }
}