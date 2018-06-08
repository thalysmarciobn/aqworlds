using System.Collections.Generic;
using AQWEmulator.Helper;
using AQWEmulator.Threading;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.World.Threads
{
    public class UserRegeneration : AbstractThread
    {
        private readonly User _user;

        public UserRegeneration(User user)
        {
            _user = user;
        }

        public override void Start()
        {
            var userState = _user.UserState;
            if (userState.Health >= userState.MaxHealth && userState.Mana >= userState.MaxMana) return;
            userState.IncreaseHealthByPercent(0.03D);
            userState.IncreaseManaByPercent(0.03D);
            var avatars = new Dictionary<string, JsonState>
            {
                {
                    _user.Name, new JsonState
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
            NetworkHelper.SendResponse(new JsonPacket(_user.RoomUser.RoomId, ct), _user.RoomUser);
            if (userState.Health >= userState.MaxHealth && userState.Mana >= userState.MaxMana)
                _user.UserState.SuspendRegenration();
        }
    }
}