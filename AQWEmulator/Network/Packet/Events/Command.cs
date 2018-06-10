using AQWEmulator.Attributes;
using AQWEmulator.World;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("cmd")]
    public class Command : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var command = parameters[0];
            switch (command)
            {
                case "tfer":
                    RoomsManager.Instance.Join(user, parameters[2]);
                    break;
                default:
                    //Logger.Warn("Command not found: " + command);
                    break;
            }
        }
    }
}