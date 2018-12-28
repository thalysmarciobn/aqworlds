using System;
using System.Collections.Generic;
using AQWEmulator.Attributes;
using AQWEmulator.Combat;
using AQWEmulator.Database;
using AQWEmulator.Database.Models;
using AQWEmulator.Helper;
using AQWEmulator.Utils;
using AQWEmulator.World.Core;
using AQWEmulator.World.Users;
using JSON;
using JSON.Command;
using JSON.Model;

namespace AQWEmulator.Network.Packet.Events
{
    [PacketIn("gar")]
    public class GetActionResult : IPacketHandler
    {
        public void Dispatch(User user, int room, string[] parameters)
        {
            var userState = user.UserState;
            if (userState.IsDead) return;
            var actId = Convert.ToInt32(parameters[0]);
            var inf = GarParser.ParseTargetInfo(parameters[1]);
            var targets = inf.Split(',');
            if (GarParser.HasDuplicate(targets))
            {
                var alertLog = new AlertLogModel
                {
                    UserId = user.Character.UserId,
                    Reason = "Duplicate targets during action.",
                    Date = DateTime.Now
                };
                GameFactory.Save(alertLog);
            }
            else
            {
                var skillReference = GarParser.GetSkillReference(parameters[1]);
                if (!user.Skills.TryGetValue(skillReference, out var skill)) return;
                if (!GarParser.IsSkillValid(skill.Reference,
                    Stats.GetRankFromPoints(user.UserClass.CharacterItem.Quantity)))
                {
                    var alertLog = new AlertLogModel
                    {
                        UserId = user.Character.UserId,
                        Reason = "Skill invalid during action.",
                        Date = DateTime.Now
                    };
                    GameFactory.Save(alertLog);
                }
                else if (userState.Mana > skill.Mana)
                {
                    var userStats = user.UserStats;
                    var fromTarget = "p:" + user.Id;
                    userState.DecreaseMana(skill.Mana);
                    if (actId % 2 == 0)
                        userState.IncreaseMana((int) (userStats.Get_V_INT() + userStats.Get_INT() / 2.0D));
                    IList<JsonGarResult> a = new List<JsonGarResult>();
                    IList<int> auras = new List<int>();
                    var p = new Dictionary<string, JsonState>();
                    var m = new Dictionary<string, JsonState>();
                    foreach (var target in targets)
                    {
                        var critChance = ServerCoreValues.Values.Configuration.BaseCritical;
                        var dodgeChance = ServerCoreValues.Values.Configuration.BaseDodge;
                        var missChance = 1.0D - (1.0D - ServerCoreValues.Values.Configuration.BaseMiss) +
                                         user.UserStats.VThi;
                        var critValue = ServerCoreValues.Values.Configuration.BaseCritical;
                        var damageEval = new DamageEval("", "", userStats.MaxDmg, userStats.MinDmg,
                            skill.Damage, critChance, dodgeChance, missChance, critValue);
                        var targetType = target.Split(':')[0];
                        var targetId = Convert.ToInt32(target.Split(':')[1]);
                        if (targetType.Equals("m"))
                            if (user.RoomUser.RoomMonsterManager.TryGet(targetId, out var mTarget))
                            {
                                var monsterState = mTarget.MonsterState;
                                var areaMonster = mTarget.AreaMonsterModel;
                                if (!monsterState.IsDead)
                                {
                                    if (monsterState.IsNeutral) monsterState.SetState(Indent.State.IntCombat);
                                    if (!user.ContainsTarget(monsterState)) user.AddTarget(monsterState);
                                    if (monsterState.IsDead) user.RemoveTarget(monsterState);
                                    userState.SetState(Indent.State.IntCombat);
                                    monsterState.AddTarget(user);
                                    monsterState.DecreaseHealth(damageEval.Damage);
                                    m.Add(areaMonster.MonMapId.ToString(), new JsonState
                                    {
                                        intHP = monsterState.Health,
                                        intMP = monsterState.Mana,
                                        intState = monsterState.State,
                                        targets = monsterState.GetTargets()
                                    });
                                }
                            }

                        var garResult = new JsonGarResult
                        {
                            cInf = fromTarget,
                            tInf = target,
                            hp = damageEval.Damage
                        };
                        if (damageEval.Type.Equals("d"))
                            garResult.typ = damageEval.Type;
                        else
                            garResult.type = damageEval.Type;
                        a.Add(garResult);
                    }

                    if (!p.ContainsKey(user.Name))
                        p.Add(user.Name, new JsonState
                        {
                            intHP = userState.Health,
                            intHPMax = userState.MaxHealth,
                            intMP = userState.Mana,
                            intMPMax = userState.MaxMana,
                            intState = userState.State
                        });
                    var ct = new JsonCt
                    {
                        p = p,
                        m = m,
                        a = auras,
                        anims = new List<JsonGarAnims>
                        {
                            new JsonGarAnims
                            {
                                strFrame = user.RoomUser.Frame,
                                fx = skill.Effects,
                                tInf = inf,
                                cInf = $"p:{user.Id}",
                                animStr = skill.Animation,
                                strl = skill.Strl
                            }
                        }
                    };
                    NetworkHelper.SendResponseToOthers(new JsonPacket(room, ct), user.RoomUser);
                    ct.sarsa = new List<JsonGarSarsa>();
                    foreach (var target in targets)
                        ct.sarsa.Add(new JsonGarSarsa
                        {
                            cInf = fromTarget,
                            tInf = target,
                            a = a,
                            actID = actId,
                            iRes = 1
                        });
                    NetworkHelper.SendResponse(new JsonPacket(room, ct), user);
                }
            }
        }
    }
}