using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AQWEmulator.World.Rooms
{
    public class RoomMonsterManager
    {
        public RoomMonsterManager(Room room)
        {
            LocalMonsters = new ConcurrentDictionary<int, RoomMonster>();
            foreach (var areaMonster in room.Area.AreaMonster)
                LocalMonsters.TryAdd(areaMonster.MonMapId,
                    new RoomMonster(room, areaMonster, areaMonster.Monster));
        }

        private ConcurrentDictionary<int, RoomMonster> LocalMonsters { get; }
        public int Count => Monsters.Count;
        public ICollection<RoomMonster> Monsters => LocalMonsters.Values;

        public bool TryGet(int monMapId, out RoomMonster _monster)
        {
            return LocalMonsters.TryGetValue(monMapId, out _monster);
        }
    }
}