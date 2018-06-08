using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using AQWEmulator.Combat;
using AQWEmulator.Database.Models;
using AQWEmulator.World.Core;
using AQWEmulator.Helper;
using JSON;
using JSON.Command;
using AQWEmulator.Threading;
using AQWEmulator.Utils;
using AQWEmulator.World.Threads;
using AQWEmulator.World.Users;

namespace AQWEmulator.World.Rooms
{
    public class RoomMonsterState : AbstractState
    {
        public RoomMonsterState(Room room, AreaMonsterModel areaMonster, MonsterModel monsterModel) : base(
            monsterModel.Heath, monsterModel.Mana)
        {
            Room = room;
            AreaMonsterModel = areaMonster;
            MonsterModel = monsterModel;
            Targets = new List<User>();
            Attacking = ThreadHelper.Schedule(new MonsterAttack(room, areaMonster, monsterModel, this), 1500);
            Regeneration = ThreadHelper.Schedule(new MonsterRegeneration(room, areaMonster, this), 1500);
        }

        private Room Room { get; }
        private AreaMonsterModel AreaMonsterModel { get; }
        private MonsterModel MonsterModel { get; }
        private IList<User> Targets { get; }
        public Timer Attacking { get; }
        public Timer Regeneration { get; }

        public void AddTarget(User connection)
        {
            if (!Targets.Contains(connection)) Targets.Add(connection);
        }

        public void RemoveTarget(User connection)
        {
            if (Targets.Contains(connection)) Targets.Remove(connection);
        }

        public int[] GetTargets()
        {
            return Targets.Select(x => x.Id).ToArray();
        }

        public User GetRandomTarget()
        {
            return Targets[new Random().Next(Targets.Count)];
        }

        public int GetTargetsCount()
        {
            return Targets.Count;
        }

        protected override void Die()
        {
            base.Die();
            var monsterModel = MonsterModel;
            var monsterAreaMonsterModel = AreaMonsterModel;
            var expDrop = monsterModel.Experience * ServerCoreValues.Settings.ExpRate;
            var goldDrop = monsterModel.Gold * ServerCoreValues.Settings.GoldRate;
            var repDrop = monsterModel.Reputation * ServerCoreValues.Settings.RepRate;
            var coinDrop = monsterModel.Coin * ServerCoreValues.Settings.CoinRate;
            var classDrop = monsterModel.ClassPoint * ServerCoreValues.Settings.ClassRate;
            foreach (var connection in Targets)
            {
                var now = DateTime.Now;
                var character = connection.Character;
                var coreExp = ServerCoreValues.GetExpToLevel(character.Level);
                var levelUp = false;
                var expBoost = character.ExpBoostExpire >= now;
                var goldBoost = character.GoldBoostExpire >= now;
                var repBoost = character.RepBoostExpire >= now;
                var coinBoost = character.CoinBoostExpire >= now;
                var classBoost = character.CpBoostExpire >= now;
                var classRank = Stats.GetRankFromPoints(connection.UserClass.CharacterItem.Quantity);
                var wonExp = character.Experience + (int) (expDrop * coreExp.Multi);
                var wonCoin = character.Coins + coinDrop;
                var wonGold = character.Gold + goldDrop;
                var wonClass = connection.UserClass.CharacterItem.Quantity + classDrop;
                var currentLevel = character.Level;
                if (expDrop > 0 && currentLevel < ServerCoreValues.Get(Indent.Core.LevelMax) && coreExp != null)
                {
                    Console.WriteLine(wonExp + " : " + coreExp.Experience);
                    while (wonExp >= coreExp.Experience)
                    {
                        wonExp -= coreExp.Experience;
                        levelUp = true;
                        currentLevel++;
                    }
                }

                if (expDrop > 0)
                {
                    if (levelUp) character.Level = currentLevel;
                    character.Experience = wonExp;
                }

                if (coinDrop > 0) character.Coins = wonCoin;
                if (goldDrop > 0 && wonGold <= ServerCoreValues.Settings.LimitGold) character.Gold = wonGold;
                if (classRank != 10 && classDrop > 0) connection.UserClass.CharacterItem.Quantity = wonClass;
                //SessionFactory.Update(character);
                //SessionFactory.Update(connection.UserClass.CharacterItem);
                if (classRank != 10 && classDrop > 0 && Stats.GetRankFromPoints(wonClass) > classRank)
                    JsonHelper.LoadSkills(connection);
                var json = new JsonAddGoldExp {id = monsterAreaMonsterModel.MonMapId};
                if (wonGold <= ServerCoreValues.Settings.LimitGold) json.intGold = goldDrop;
                if (wonCoin <= ServerCoreValues.Settings.LimitCoin) json.intCoin = coinDrop;
                if (expDrop > 0 && currentLevel < ServerCoreValues.Get(Indent.Core.LevelMax))
                    json.intExp = levelUp ? wonExp : expDrop;
                if (classRank != 10 && classDrop > 0) json.iCP = classDrop;
                if (monsterModel.FactionId > 0)
                {
                    json.FactionID = monsterModel.FactionId;
                    json.iRep = repDrop;
                }

                json.typ = "m";
                NetworkHelper.SendResponse(new JsonPacket(-1, json), connection);
                if (levelUp)
                    NetworkHelper.SendResponse(
                        new JsonPacket(-1,
                            new JsonLevelUp
                            {
                                intLevel = currentLevel,
                                intExpToLevel = ServerCoreValues.GetExpToLevel(currentLevel).Experience
                            }), connection);
                connection.RemoveTarget(this);
            }

            ThreadHelper.ExecuteWithDelay(new MonsterRespawn(Room, this, monsterAreaMonsterModel),
                monsterModel.Respawn);
            Targets.Clear();
        }

        public override void SetState(int state)
        {
            base.SetState(state);
            if (IsNeutral)
            {
                if (Health < MaxHealth || Mana < MaxMana) Regeneration.Start();
                Attacking.Stop();
            }
            else if (OnCombat)
            {
                Attacking.Start();
            }
            else
            {
                Regeneration.Stop();
                Attacking.Stop();
            }
        }
    }
}