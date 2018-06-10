using System;
using System.Collections.Concurrent;
using System.Threading;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.World.Rooms;
using AQWEmulator.World.Users;

namespace AQWEmulator.World
{
    public class RoomsManager
    {
        private static RoomsManager _instance;
        private readonly ConcurrentDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();
        private readonly object _lock = new object();
        private int _lastRoomId = 2;

        public static RoomsManager Instance => _instance ?? (_instance = new RoomsManager());

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _rooms.Count;
                }
            }
        }

        public void Remove(string name)
        {
            lock (_lock)
            {
                if (!_rooms.TryRemove(name, out var room)) return;
                foreach (var monster in room.MonsterManager.Monsters)
                {
                    monster.MonsterState.Attacking.Dispose();
                    monster.MonsterState.Regeneration.Dispose();
                }
                Interlocked.Decrement(ref _lastRoomId);
            }
        }

        public void Join(User user, string roomName, string frame = "Enter", string pad = "Spawn", bool first = false)
        {
            var nRoom = !roomName.Contains("-") ? roomName + "-1" : roomName;
            using (var session = GameFactory.Session.OpenSession())
            {
                Console.WriteLine(roomName);
                var area = session.QueryOver<AreaModel>()
                    .Where(x => x.Name == (roomName.Contains("-") ? roomName.Split('-')[0] : roomName))
                    .SingleOrDefault();
                if (area == null) return;
                Console.WriteLine(area.File);
                if (first)
                {
                    lock (_lock)
                    {
                        if (_rooms.TryGetValue(nRoom, out var room))
                        {
                            if (room.UserManager.Count >= room.Area.MaxPlayers)
                            {
                                room = GenerateRoom(nRoom);
                                user.RoomUser.Enter(room, frame, pad);
                                room.UserManager.Join(user);
                            }
                            else
                            {
                                room = _rooms.GetOrAdd(nRoom, new Room(this, nRoom, Interlocked.Increment(ref _lastRoomId)));
                                user.RoomUser.Enter(room, frame, pad);
                                room.UserManager.Join(user);
                            }
                        }
                        else
                        {
                            room = _rooms.GetOrAdd(nRoom, new Room(this, nRoom, Interlocked.Increment(ref _lastRoomId)));
                            user.RoomUser.Enter(room, frame, pad);
                            room.UserManager.Join(user);
                        }
                    }
                }
                else if (!roomName.Contains("-") && user.RoomUser.Remove())
                {
                    var room = GenerateRoom(nRoom);
                    user.RoomUser.Enter(room, frame, pad);
                    room.UserManager.Join(user);
                }
                else
                {
                    lock (_lock)
                    {
                        if (area.Staff == 1 && user.Character.Access >= 40)
                        {
                            NetworkHelper.SendResponse(
                                new[] {"xt", "warning", "-1", "\"" + area.Name + "\" is staff only."},
                                user);
                        }
                        else if (area.ReqLevel > user.Character.Level)
                        {
                            NetworkHelper.SendResponse(
                                new[]
                                {
                                    "xt", "warning", "-1",
                                    "\"" + area.Name + "\" requires level " +
                                    area.ReqLevel + " and above to enter."
                                }, user);
                        }
                        else if (area.Upgrade == 1 && user.Character.UpgradeExpire >= DateTime.Now)
                        {
                            NetworkHelper.SendResponse(
                                new[] {"xt", "warning", "-1", "\"" + area.Name + "\" is member only."},
                                user);
                        }
                        else if (_rooms.TryGetValue(nRoom, out var room))
                        {
                            if (room.Id == user.RoomUser.RoomId)
                            {
                                NetworkHelper.SendResponse(
                                    new[] {"xt", "warning", "-1", "Cannot join a room you are currently in!"}, user);
                            }
                            else if (room.UserManager.Count >= room.Area.MaxPlayers)
                            {
                                NetworkHelper.SendResponse(
                                    new[] {"xt", "warning", "-1", "Room join failed, destination room is full."}, user);
                            }
                            else if (user.RoomUser.Remove())
                            {
                                user.RoomUser.Enter(room, frame, pad);
                                room.UserManager.Join(user);
                            }
                        }
                        else if (user.RoomUser.Remove())
                        {
                            var newRoom = _rooms.GetOrAdd(nRoom, new Room(this, nRoom, Interlocked.Increment(ref _lastRoomId)));
                            user.RoomUser.Enter(newRoom, frame, pad);
                            newRoom.UserManager.Join(user);
                        }
                    }
                }
            }
        }

        private Room GenerateRoom(string roomName)
        {
            var parameters = roomName.Split('-');
            var roomK = parameters[0];
            var roomV = int.Parse(parameters[1]);
            while (true)
                lock (_lock)
                {
                    if (_rooms.TryGetValue(roomK + "-" + roomV, out var room))
                    {
                        if (room.UserManager.Count < room.Area.MaxPlayers) return room;
                        roomV++;
                    }
                    else
                    {
                        return _rooms.GetOrAdd(roomK + "-" + roomV, new Room(this, roomK + "-" + roomV, Interlocked.Increment(ref _lastRoomId)));
                    }
                }
        }
    }
}