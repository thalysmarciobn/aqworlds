using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.Threading;
using AQWEmulator.World.Rooms;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.World.Threads
{
    public class MonsterRespawn : AbstractThread
    {
        private readonly AreaMonsterModel _areaMonster;
        private readonly Room _room;
        private readonly RoomMonsterState _state;

        public MonsterRespawn(Room roon, RoomMonsterState state, AreaMonsterModel areaMonster)
        {
            _room = roon;
            _state = state;
            _areaMonster = areaMonster;
        }

        public override void Start()
        {
            _state.Restore();
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
            NetworkHelper.SendResponse(new[] {"xt", "respawnMon", "-1", _areaMonster.MonMapId.ToString()},
                _room.UserManager);
        }
    }
}