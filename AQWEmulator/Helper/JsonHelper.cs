using System;
using JSON;
using JSON.Command;
using JSON.Model;
using Newtonsoft.Json.Linq;
using AQWEmulator.Combat;
using AQWEmulator.Utils;
using AQWEmulator.World.Core;
using AQWEmulator.World.Users;

namespace AQWEmulator.Helper
{
    public static class JsonHelper
    {
        public static void SendStats(User user, bool levelUp)
        {
            user.UserStats.Update();
            var END = Convert.ToInt32(user.UserStats.Get_V_END() + user.UserStats.Get_END());
            var WIS = Convert.ToInt32(user.UserStats.Get_V_WIS() + user.UserStats.Get_WIS());
            var intHPperEND = Convert.ToInt32(ServerCoreValues.Get(Indent.Core.IntHpPerEnd));
            var intMPperWIS = Convert.ToInt32(ServerCoreValues.Get(Indent.Core.IntHpPerWis));
            var addedHP = END * intHPperEND;
            var userHp = (int) (ServerCoreValues.GetHealthByLevel(user.Character.Level) + addedHP);
            const int
                userMp = 100; //(int)(Emulator.Server.Core.GetManaByLevel(this.UserModel.Character.Level) + WIS * intMPperWIS);
            user.UserState.SetMaxHealth(userHp);
            user.UserState.SetMaxMana(userMp);
            if (user.UserState.State == 1 || levelUp)
            {
                user.UserState.SetHealth(userHp);
                user.UserState.SetMana(userMp);
            }

            var uotls = new JsonUotls
            {
                o = new JsonUoBranch
                {
                    intHP = user.UserState.Health,
                    intMP = user.UserState.Mana,
                    intLevel = levelUp ? user.Character.Level : 0
                },
                unm = user.Name
            };
            NetworkHelper.SendResponse(new JsonPacket(-1, uotls), user.RoomUser);
            var stu = new JsonStu
            {
                wDPS = user.UserStats.WDps,
                mDPS = user.UserStats.MDps,
                sta = new JsonStuSta
                {
                    _tha = user.UserStats.Tha,
                    _srm = user.UserStats.Srm,
                    _tcr = user.UserStats.Tcr,
                    _tpa = user.UserStats.Tpa,
                    _sbm = user.UserStats.Sbm,
                    _thi = user.UserStats.Thi,
                    _cai = user.UserStats.Cai,
                    _smb = user.UserStats.Smb,
                    _cmo = user.UserStats.Cmo,
                    _cmi = user.UserStats.Cmi,
                    _sem = user.UserStats.Sem,
                    _cao = user.UserStats.Cao,
                    _shb = user.UserStats.Shb,
                    _cdo = user.UserStats.Cdo,
                    _tre = user.UserStats.Tre,
                    _cdi = user.UserStats.Cdi,
                    _cho = user.UserStats.Cho,
                    _chi = user.UserStats.Chi,
                    _ap = user.UserStats.Ap,
                    _cpo = user.UserStats.Cpo,
                    _tbl = user.UserStats.Tbl,
                    _cmc = user.UserStats.Cmc,
                    _tdo = user.UserStats.Tdo,
                    _cpi = user.UserStats.Cpi,
                    _scm = user.UserStats.Scm,
                    _sp = user.UserStats.Sp,
                    v_tha = user.UserStats.VTha,
                    v_srm = user.UserStats.VSrm,
                    v_tcr = user.UserStats.VTcr,
                    v_tpa = user.UserStats.VTpa,
                    v_sbm = user.UserStats.VSbm,
                    v_thi = user.UserStats.VThi,
                    v_cai = user.UserStats.VCai,
                    v_smb = user.UserStats.VSmb,
                    v_cmo = user.UserStats.VCmo,
                    v_cmi = user.UserStats.VCmi,
                    v_sem = user.UserStats.VSem,
                    v_cao = user.UserStats.VCao,
                    v_shb = user.UserStats.VShb,
                    v_cdo = user.UserStats.VCdo,
                    v_tre = user.UserStats.VTre,
                    v_cdi = user.UserStats.VCdi,
                    v_cho = user.UserStats.VCho,
                    v_chi = user.UserStats.VChi,
                    v_ap = user.UserStats.VAp,
                    v_cpo = user.UserStats.VCpo,
                    v_tbl = user.UserStats.VTbl,
                    v_cmc = user.UserStats.VCmc,
                    v_tdo = user.UserStats.VTdo,
                    v_cpi = user.UserStats.VCpi,
                    v_scm = user.UserStats.VScm,
                    v_sp = user.UserStats.VSp,
                    _INT = user.UserStats.Get_INT(),
                    _STR = user.UserStats.Get_STR(),
                    _DEX = user.UserStats.Get_DEX(),
                    _END = user.UserStats.Get_END(),
                    _LCK = user.UserStats.Get_LCK(),
                    _WIS = user.UserStats.Get_WIS(),
                    V_INT = user.UserStats.Get_V_INT(),
                    V_STR = user.UserStats.Get_V_STR(),
                    V_DEX = user.UserStats.Get_V_DEX(),
                    V_END = user.UserStats.Get_V_END(),
                    V_LCK = user.UserStats.Get_V_LCK(),
                    V_WIS = user.UserStats.Get_V_WIS(),
                    minDmg = user.UserStats.MinDmg,
                    maxDmg = user.UserStats.MaxDmg
                },
                tempSta = new JsonStuTempSta
                {
                    ba = user.UserStats.Cape,
                    ar = user.UserStats.Armor,
                    he = user.UserStats.Helm,
                    Weapon = user.UserStats.Weapon,
                    innate = new JsonStuTempStaInnate
                    {
                        INT = user.UserStats.Get_INT(),
                        STR = user.UserStats.Get_STR(),
                        DEX = user.UserStats.Get_DEX(),
                        END = user.UserStats.Get_END(),
                        LCK = user.UserStats.Get_LCK(),
                        WIS = user.UserStats.Get_WIS()
                    }
                }
            };
            NetworkHelper.SendResponse(new JsonPacket(-1, stu), user);
        }

        public static void UpdateClass(User user)
        {
            var updateClass = new JObject();
            updateClass.Add("cmd", "updateClass");
            updateClass.Add("iCP", user.UserClass.CharacterItem.Quantity);
            updateClass.Add("sClassCat", user.UserClass.ItemModel.ItemClass.Category);
            updateClass.Add("uid", user.Id);
            updateClass.Add("sClassName", user.UserClass.ItemModel.Name);
            NetworkHelper.SendResponseToOthers(updateClass, user.RoomUser);
            updateClass.Add("sDesc", user.UserClass.ItemModel.Description);
            updateClass.Add("sStats", user.UserClass.ItemModel.ItemClass.StatsDescription);
            if (user.UserClass.ItemModel.ItemClass.ManaRegenerationMethods.Contains(":"))
            {
                var aMRM = new JArray();
                var _params = user.UserClass.ItemModel.ItemClass.ManaRegenerationMethods.Split('|');
                foreach (var method in _params) aMRM.Add(method + "\r");
                updateClass.Add("aMRM", aMRM);
            }
            else
            {
                updateClass.Add("aMRM", user.UserClass.ItemModel.ItemClass.ManaRegenerationMethods);
            }

            NetworkHelper.SendResponse(updateClass, user);
        }

        public static void LoadSkills(User user)
        {
            var rank = Stats.GetRankFromPoints(user.UserClass.CharacterItem.Quantity);
            var active = new JArray();
            var passive = new JArray();
            var sAct = new JObject();
            foreach (var classSkill in user.UserClass.ItemModel.ItemClass.Skills)
            {
                var skill = classSkill.Skill;
                user.Skills[skill.Reference] = skill;
                if (skill.Type.Equals("passive"))
                {
                    var actObj = new JObject
                    {
                        {"desc", skill.Description},
                        {"fx", skill.Effects},
                        {"icon", skill.Icon},
                        {"id", skill.Id},
                        {"nam", skill.Name},
                        {"range", skill.Range},
                        {"ref", skill.Reference},
                        {"tgt", skill.Target},
                        {"typ", skill.Type}
                    };
                    var arrAuras = new JArray();
                    arrAuras.Add(new JObject());
                    actObj.Add("auras", arrAuras);
                    actObj.Add("isOK", rank >= 4);
                    passive.Add(actObj);
                }
                else
                {
                    var actObj = new JObject
                    {
                        {"anim", skill.Animation},
                        {"cd", skill.Cooldown},
                        {"damage", skill.Damage},
                        {"desc", skill.Description}
                    };
                    if (!skill.Dsrc.Equals("")) actObj.Add("dsrc", skill.Dsrc);
                    actObj.Add("fx", skill.Effects);
                    actObj.Add("icon", skill.Icon);
                    actObj.Add("id", skill.Id);
                    actObj.Add("mp", skill.Mana);
                    actObj.Add("nam", skill.Name);
                    actObj.Add("range", skill.Range);
                    actObj.Add("ref", skill.Reference);
                    if (!skill.Strl.Equals("")) actObj.Add("strl", skill.Strl);
                    actObj.Add("tgt", skill.Target);
                    actObj.Add("typ", skill.Type);

                    if (skill.HitTargets > 0)
                    {
                        actObj.Add("tgtMax", skill.HitTargets);
                        actObj.Add("tgtMin", "1");
                    }

                    switch (skill.Reference)
                    {
                        case "aa":
                            actObj["typ"] = "aa";
                            actObj.Add("auto", true);
                            actObj.Add("isOK", true);
                            active.Add(actObj);
                            break;
                        case "a1":
                            actObj.Add("isOK", true);
                            active.Add(actObj);
                            break;
                        case "a2":
                            actObj.Add("isOK", rank >= 2);
                            active.Add(actObj);
                            break;
                        case "a3":
                            actObj.Add("isOK", rank >= 3);
                            active.Add(actObj);
                            break;
                        case "a4":
                            actObj.Add("isOK", rank >= 5);
                            active.Add(actObj);
                            break;
                    }
                }
            }

            var potionObj1 = new JObject
            {
                {"anim", "Cheer"},
                {"cd", "60000"},
                {"desc", "Equip a potion or scroll from your inventory to use it here."},
                {"fx", ""},
                {"icon", "icu1"},
                {"isOK", true},
                {"mp", "0"},
                {"nam", "Potions"},
                {"range", 808},
                {"ref", "i1"},
                {"str1", ""},
                {"tgt", "f"},
                {"typ", "i"}
            };
            active.Add(potionObj1);
            var actions1 = new JObject {{"active", active}, {"passive", passive}};
            sAct.Add("cmd", "sAct");
            sAct.Add("actions", actions1);
            ClearAuras(user);
            ApplyPassiveAuras(user, rank);
            NetworkHelper.SendResponse(sAct, user);
        }

        public static void ClearAuras(User user)
        {
            user.UserStats.Effects.Clear();
        }

        public static void ApplyPassiveAuras(User user, int rank)
        {
            if (rank >= 4)
            {
            }
        }
    }
}