using AQWEmulator.Database.Models;

namespace AQWEmulator.World.Rooms
{
    public class RoomMonster
    {
        public RoomMonster(Room room, AreaMonsterModel areaMonster, MonsterModel monsterModel)
        {
            AreaMonsterModel = areaMonster;
            MonsterModel = monsterModel;
            MonsterState = new RoomMonsterState(room, areaMonster, monsterModel);
        }

        public AreaMonsterModel AreaMonsterModel { get; }
        public MonsterModel MonsterModel { get; }
        public RoomMonsterState MonsterState { get; }
    }
}