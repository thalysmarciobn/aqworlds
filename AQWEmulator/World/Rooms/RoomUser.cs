using System;
using System.Collections.Generic;
using AQWEmulator.Database.Models;
using AQWEmulator.World.Users;

namespace AQWEmulator.World.Rooms
{
    public class RoomUser
    {
        private readonly User _user;
        private Room _room;

        public RoomUser(User user)
        {
            _user = user;
            Frame = "Enter";
            Pad = "Spawn";
            Tx = 0;
            Ty = 0;
            Afk = false;
            Targets = new List<object>();
            RespawnTime = DateTime.Now;
        }

        public string RoomName => _room != null ? _room.Name : "";

        public string Name => _user.Name;

        public string Frame { get; set; }
        public string Pad { get; set; }

        public int RoomId => _room?.Id ?? 0;

        public int UserId => _user.Id;

        public int Tx { get; set; }
        public int Ty { get; set; }
        public bool Afk { get; set; }

        public bool IsPVP => _room != null && _room.IsPVP;

        public CharacterModel Character => _user.Character;
        public UserState UserState => _user.UserState;
        public RoomUserManager RoomUserManager => _room.UserManager;
        public RoomMonsterManager RoomMonsterManager => _room.MonsterManager;
        public IList<object> Targets { get; protected set; }
        public DateTime RespawnTime { get; set; }

        public void Enter(Room room, string frame, string pad)
        {
            _room = room;
            Frame = frame;
            Pad = pad;
            Tx = 0;
            Ty = 0;
        }

        public void Send(string message)
        {
            _user.Send(message);
        }

        public bool Remove()
        {
            return _room != null && _room.UserManager.Remove(this);
        }
    }
}