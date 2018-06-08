using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using AQWEmulator.Database.Models;
using AQWEmulator.Network.Sessions;
using AQWEmulator.Utils;
using AQWEmulator.World.Rooms;
using JSON.Model;

namespace AQWEmulator.World.Users
{
    public class User
    {
        public User(int id, string name, UserSession session, CharacterModel character)
        {
            _targets = new List<object>();
            Id = id;
            PartyId = 0;
            PartyOwnerId = 0;
            Name = name;
            Character = character;
            Channel = session;
            UserClass = new UserClass();
            UserState = new UserState(this, 100, 100);
            UserStats = new UserStats(this);
            Skills = new Dictionary<string, SkillModel>();
            Equipment = new Dictionary<string, JsonUserEquipment>();
            RoomUser = new RoomUser(this);
        }

        private IList<object> _targets { get; }
        public Dictionary<string, SkillModel> Skills { get; }
        public Dictionary<string, JsonUserEquipment> Equipment { get; set; }
        public UserClass UserClass { get; }
        public CharacterModel Character { get; }
        public UserState UserState { get; }
        public UserStats UserStats { get; }
        public RoomUser RoomUser { get; }
        private UserSession Channel { get; }
        public string Name { get; }
        public int Id { get; }
        public long PartyId { get; set; }
        public long PartyOwnerId { get; set; }

        public object[] Targets => _targets.ToArray();

        public bool IsBusy => UserState.OnCombat || RoomUser.IsPVP;

        public void Send(string message)
        {
            Console.WriteLine("[SEND] : " + message);
            var data = Encoding.UTF8.GetBytes(message + Convert.ToChar(0x0));
            Channel.Send(data);
        }

        public bool ContainsTarget(object target)
        {
            lock (_targets)
            {
                return _targets.Contains(target);
            }
        }

        public void AddTarget(object target)
        {
            lock (_targets)
            {
                _targets.Add(target);
                UserState.SetState(Indent.State.IntCombat);
            }
        }

        public void RemoveTarget(object target)
        {
            lock (_targets)
            {
                _targets.Remove(target);
                if (_targets.Count <= 0) UserState.SetState(Indent.State.IntNeutral);
            }
        }

        public void ClearTargets()
        {
            lock (_targets)
            {
                _targets.Clear();
            }
        }
    }
}