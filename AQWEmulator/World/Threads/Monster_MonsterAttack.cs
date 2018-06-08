using System;
using System.Collections.Generic;
using AQWEmulator.Combat;
using AQWEmulator.Database.Models;
using AQWEmulator.World.Core;
using AQWEmulator.Helper;
using AQWEmulator.Threading;
using AQWEmulator.Utils;
using AQWEmulator.World.Rooms;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.World.Threads
{
    public class MonsterAttack : AbstractThread
    {
        public MonsterAttack(Room room, AreaMonsterModel areaMonster, MonsterModel monsterModel,
            RoomMonsterState state)
        {
            Room = room;
            AreaMonster = areaMonster;
            MonsterModel = monsterModel;
            State = state;
        }

        private Room Room { get; }
        private AreaMonsterModel AreaMonster { get; }
        private MonsterModel MonsterModel { get; }
        private RoomMonsterState State { get; }

        public override void Start()
        {
            Attack();
        }

        private void Attack()
        {
            if (!State.OnCombat) return;
            var user = State.GetRandomTarget();
            if (Room.Id.Equals(user.RoomUser.RoomId) && AreaMonster.Frame.Equals(user.RoomUser.Frame))
            {
                DoAttack(user);
            }
            else
            {
                State.RemoveTarget(user);
                if (State.GetTargetsCount() <= 0) State.SetState(Indent.State.IntNeutral);
            }
        }

        private void DoAttack(User user)
        {
            var userState = user.UserState;
            if (userState.IsNeutral && !State.IsDead) userState.SetState(Indent.State.IntCombat);
            if (!State.OnCombat) return;
            var dps = MonsterModel.Dps;
            var maxDmg = (int) Math.Ceiling(dps + dps * 0.1D);
            var minDmg = (int) Math.Floor(dps - dps * 0.1D);
            var critChance = MonsterModel.Level / ServerCoreValues.Get(Indent.Core.LevelMax);
            var dodgeChance = user.UserStats.VTdo;
            var missChance = ServerCoreValues.Get(Indent.Core.BaseMiss);
            var critValue = ServerCoreValues.Get(Indent.Core.BaseCritValue);
            var damageEval = new DamageEval("", "", maxDmg, minDmg, 1, critChance, dodgeChance, missChance, critValue);
            userState.DecreaseHealth(damageEval.Damage);
            var ct = new JsonCt
            {
                p = new Dictionary<string, JsonState>
                {
                    {
                        user.Name, new JsonState
                        {
                            intHP = userState.Health,
                            intHPMax = userState.MaxHealth,
                            intMP = userState.Mana,
                            intMPMax = userState.MaxMana,
                            intState = userState.State
                        }
                    }
                },
                m = new Dictionary<string, JsonState>
                {
                    {
                        AreaMonster.MonMapId.ToString(), new JsonState
                        {
                            intHP = State.Health,
                            intMP = State.Mana,
                            intState = State.State
                        }
                    }
                },
                anims = new List<JsonGarAnims>
                {
                    new JsonGarAnims
                    {
                        strFrame = AreaMonster.Frame,
                        tInf = $"p:{user.Id}",
                        cInf = $"m:{AreaMonster.MonMapId}",
                        animStr = "Attack1,Attack2"
                    }
                }
            };
            NetworkHelper.SendResponseToOthers(new JsonPacket(Room.Id, ct), user.RoomUser);
            ct.sara = new List<JsonGarSara>
            {
                new JsonGarSara
                {
                    actionResult = new JsonGarActionResult
                    {
                        tInf = $"p:{user.Id}",
                        cInf = $"m:{AreaMonster.MonMapId}",
                        type = damageEval.Type,
                        hp = damageEval.Damage
                    },
                    iRes = 1
                }
            };
            NetworkHelper.SendResponse(new JsonPacket(Room.Id, ct), user);
            if (!userState.IsDead) return;
            State.RemoveTarget(user);
            if (State.GetTargetsCount() <= 0) State.SetState(Indent.State.IntNeutral);
        }
    }
}