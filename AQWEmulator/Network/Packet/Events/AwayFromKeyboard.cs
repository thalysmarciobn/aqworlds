using AQWEmulator.Helper;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("afk")]
    public class AwayFromKeyboard : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (!bool.TryParse(parameters[0], out var afk)) return;
            if (afk == user.RoomUser.Afk) return;
            NetworkHelper.SendResponse(
                !afk
                    ? new[] {"xt", "server", "-1", "You are no longer Away From Keyboard (AFK)."}
                    : new[] {"xt", "server", "-1", "You are now Away From Keyboard (AFK)."}, user);
            NetworkHelper.SendResponse(
                new[] {"xt", "uotls", "-1", user.Name, "afk:" + afk.ToString().ToLower()}, user.RoomUser);
            user.RoomUser.Afk = afk;
        }
    }
}