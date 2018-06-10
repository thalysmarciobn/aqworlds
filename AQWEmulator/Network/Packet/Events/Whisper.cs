using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("whisper")]
    public class Whisper : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var message = parameters[0];
            var target = parameters[1].ToLower();
            if (!UsersManager.Instance.ContainsUserByName(target))
            {
            }
            else
            {
                var client = UsersManager.Instance.GetUserByName(target);
                if (client.IsBusy)
                {
                }
                else
                {
                    NetworkHelper.SendResponse(new[] {"xt", "whisper", "-1", message, user.Name, target, "0"},
                        new[] {user, client});
                }
            }
        }
    }
}