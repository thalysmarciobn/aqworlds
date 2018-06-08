using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.Threading;
using AQWEmulator.World.Rooms;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.World.Threads
{
    public class MonsterRegeneration : AbstractThread
    {
        private readonly AreaMonsterModel _areaMonster;
        private readonly Room _room;
        private readonly RoomMonsterState _state;

        public MonsterRegeneration(Room roon, AreaMonsterModel AreaMonster, RoomMonsterState State)
        {
            _room = roon;
            _areaMonster = AreaMonster;
            _state = State;
        }

        public override void Start()
        {
            if (_state.GetTargetsCount() <= 0 && _state.IsNeutral &&
                (_state.Health < _state.MaxHealth || _state.Mana < _state.MaxMana))
            {
                _state.IncreaseHealthByPercent(0.09D);
                _state.IncreaseManaByPercent(0.09D);
                var mtls = new JsonMtls
                {
                    id = _areaMonster.MonMapId,
                    o = new JsonState
                    {
                        intHP = _state.Health,
                        intMP = _state.Mana,
                        intState = _state.State
                    }
                };
                NetworkHelper.SendResponse(new JsonPacket(_room.Id, mtls), _room.UserManager);
            }
            else
            {
                _state.Regeneration.Stop();
            }
        }
    }
}