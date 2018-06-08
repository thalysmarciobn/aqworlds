using AQWEmulator.Helper;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("restRequest")]
    public class RestRequest : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var userState = user.UserState;
            if (!userState.IsNeutral ||
                userState.Health >= userState.MaxHealth && userState.Mana >= userState.MaxMana) return;
            userState.IncreaseHealthByPercent(0.15D);
            userState.IncreaseManaByPercent(0.15D);
            var uotls = new JsonUotls
            {
                o = new JsonUoBranch
                {
                    intHP = userState.Health,
                    intMP = userState.Mana
                },
                unm = user.Name
            };
            NetworkHelper.SendResponse(new JsonPacket(-1, uotls), user);
        }
    }
}