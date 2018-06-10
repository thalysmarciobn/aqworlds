using System.Collections.Generic;
using AQWEmulator.Attributes;
using AQWEmulator.Helper;
using AQWEmulator.Utils;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("moveToCell")]
    public class MoveToCell : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var frame = parameters[0];
            var pad = parameters[1];
            user.RoomUser.Frame = frame;
            user.RoomUser.Pad = pad;
            user.RoomUser.Tx = 0;
            user.RoomUser.Ty = 0;
            user.ClearTargets();
            var userState = user.UserState;
            if (!userState.IsNeutral)
            {
                userState.SetState(Indent.State.IntNeutral);
                var avatars = new Dictionary<string, JsonState>
                {
                    {
                        user.Name, new JsonState
                        {
                            intHP = userState.Health,
                            intHPMax = userState.MaxHealth,
                            intMP = userState.Mana,
                            intMPMax = userState.MaxMana,
                            intState = userState.State
                        }
                    }
                };
                var ct = new JsonCt
                {
                    p = avatars
                };
                NetworkHelper.SendResponse(new JsonPacket(user.RoomUser.RoomId, ct), user.RoomUser);
            }

            NetworkHelper.SendResponseToOthers(
                new[] {"xt", "uotls", "-1", user.Name, "strPad:" + pad + ",tx:0,strFrame:" + frame + ",ty:0"},
                user.RoomUser);
        }
    }
}