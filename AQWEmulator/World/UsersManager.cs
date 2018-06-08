using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using AQWEmulator.Database.Models;
using AQWEmulator.Network.Sessions;
using AQWEmulator.World.Users;

namespace AQWEmulator.World
{
    public class UsersManager
    {
        private static UsersManager _instance;
        private readonly object _lock = new object();
        private readonly ConcurrentDictionary<string, User> _users = new ConcurrentDictionary<string, User>();
        private int _lastUserId = 2;

        public static UsersManager Instance => _instance ?? (_instance = new UsersManager());

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _users.Count;
                }
            }
        }

        public User AddAndGet(string name, Socket channel, CharacterModel character)
        {
            lock (_lock)
            {
                var user = new User(Interlocked.Increment(ref _lastUserId), name.ToLower(), channel, character);
                if (_users.TryAdd(name, user)) return user;
            }

            return null;
        }

        public void Remove(User user)
        {
            lock (_lock)
            {
                Interlocked.Decrement(ref _lastUserId);
                _users.TryRemove(user.Name, out user);
            }
        }

        public bool ContainsUserByName(string name)
        {
            lock (_users)
            {
                return _users.SingleOrDefault(x => x.Key == name).Key != null;
            }
        }

        public User GetUserById(int id)
        {
            lock (_lock)
            {
                return _users.SingleOrDefault(x => x.Value.Id == id).Value;
            }
        }

        public User GetUserByName(string name)
        {
            lock (_lock)
            {
                return _users.SingleOrDefault(x => x.Value.Name == name).Value;
            }
        }

        public IEnumerable<User> GetUsersByAccess(int access)
        {
            lock (_lock)
            {
                return _users.Where(x => x.Value.Character.Access == access).Select(x => x.Value);
            }
        }

        public IEnumerable<User> GetUsersUpgraded()
        {
            lock (_lock)
            {
                return _users.Where(x => x.Value.Character.UpgradeExpire >= DateTime.Now).Select(x => x.Value);
            }
        }
    }
}