using System;
using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("resPlayerTimed")]
    public class RestorePlayerTimed : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            if (TimeSpan.FromTicks((DateTime.Now - user.RoomUser.RespawnTime).Ticks).Seconds < 8) return;
            var userState = user.UserState;
            userState.Restore();

            var uotls = new JsonUotls
            {
                o = new JsonUoBranch
                {
                    intHP = userState.Health,
                    intMP = userState.Mana,
                    intState = userState.State
                },
                unm = user.Name
            };
            NetworkHelper.SendResponse(new JsonPacket(-1, uotls), user.RoomUser);
            NetworkHelper.SendResponse(new[] {"xt", "resTimed", "-1", "Enter", "Spawn"}, user);
        }
    }
}