using System;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;

namespace AQWEmulator.World.Rooms
{
    public class Room
    {
        public Room(RoomsManager roomsManager, string roomName, int roomId)
        {
            using (var session = GameFactory.Session.OpenSession())
            {
                Area = session.QueryOver<AreaModel>()
                    .Where(x => x.Name == (roomName.Contains("-") ? roomName.Split('-')[0] : roomName))
                    .SingleOrDefault();
                if (Area == null) return;
                Name = roomName;
                Id = roomId;
                UserManager = new RoomUserManager(roomsManager, this);
                MonsterManager = new RoomMonsterManager(this);
                IsPvp = Convert.ToBoolean(Area.Pvp);
            }
        }

        public AreaModel Area { get; }

        public string Name { get; }
        public int Id { get; }
        public bool IsPvp { get; }
        public RoomUserManager UserManager { get; }
        public RoomMonsterManager MonsterManager { get; }
    }
}