using System;
using System.Collections.Generic;
using AQWEmulator.Combat;
using AQWEmulator.Database.Models;
using AQWEmulator.Utils;
using AQWEmulator.World.Core;

namespace AQWEmulator.World.Users
{
    public class UserStats
    {
        public readonly Dictionary<string, int> Armor =
            new Dictionary<string, int> {{"STR", 0}, {"END", 0}, {"DEX", 0}, {"INT", 0}, {"WIS", 0}, {"LCK", 0}};

        public readonly Dictionary<string, int> Cape =
            new Dictionary<string, int> {{"STR", 0}, {"END", 0}, {"DEX", 0}, {"INT", 0}, {"WIS", 0}, {"LCK", 0}};

        public readonly HashSet<SkillAuraEffectModel> Effects = new HashSet<SkillAuraEffectModel>();

        public readonly Dictionary<string, int> Helm =
            new Dictionary<string, int> {{"STR", 0}, {"END", 0}, {"DEX", 0}, {"INT", 0}, {"WIS", 0}, {"LCK", 0}};

        private readonly Dictionary<string, int> Innate =
            new Dictionary<string, int> {{"STR", 0}, {"END", 0}, {"DEX", 0}, {"INT", 0}, {"WIS", 0}, {"LCK", 0}};

        public readonly Dictionary<string, int> Weapon =
            new Dictionary<string, int> {{"STR", 0}, {"END", 0}, {"DEX", 0}, {"INT", 0}, {"WIS", 0}, {"LCK", 0}};

        public UserStats(User user)
        {
            User = user;
            WDps = 0;
            MDps = 0;
            VCai = 1.0D;
            VCao = 1.0D;
            VCdi = 1.0D;
            VCdo = 1.0D;
            VChi = 1.0D;
            VCho = 1.0D;
            VCmc = 1.0D;
            VCmi = 1.0D;
            VCmo = 1.0D;
            VCpi = 1.0D;
            VCpo = 1.0D;
            VSbm = 0.7D;
            VScm = 1.5D;
            VSem = 0.05D;
            VShb = 0.0D;
            VSmb = 0.0D;
            VSrm = 0.7D;
            VAp = 0.0D;
            VSp = 0.0D;
            VTbl = 0.0D;
            VTcr = 0.05D;
            VTdo = 0.04D;
            VTha = 0.0D;
            VThi = 0.0D;
            VTpa = 0.03D;
            VTre = 0.7D;
            Ap = 0.0D;
            Cai = 1.0D;
            Cao = 1.0D;
            Cdi = 1.0D;
            Cdo = 1.0D;
            Chi = 1.0D;
            Cho = 1.0D;
            Cmc = 1.0D;
            Cmi = 1.0D;
            Cmo = 1.0D;
            Cpi = 1.0D;
            Cpo = 1.0D;
            Sbm = 0.7D;
            Scm = 1.5D;
            Sem = 0.05D;
            Shb = 0.0D;
            Smb = 0.0D;
            Sp = 0.0D;
            Srm = 0.7D;
            Tbl = 0.0D;
            Tcr = 0.0D;
            Tdo = 0.0D;
            Tha = 0.0D;
            Thi = 0.0D;
            Tpa = 0.0D;
            Tre = 0.0D;
            MinDmg = 0;
            MaxDmg = 0;
        }

        private User User { get; }
        public ItemModel WeaponModel { get; set; }
        public EnhancementModel WeaponEnhancementModel { get; set; }
        public int WDps { get; private set; }
        public int MDps { get; private set; }
        public double VCai { get; private set; }
        public double VCao { get; private set; }
        public double VCdi { get; private set; }
        public double VCdo { get; private set; }
        public double VChi { get; private set; }
        public double VCho { get; private set; }
        public double VCmc { get; private set; }
        public double VCmi { get; private set; }
        public double VCmo { get; private set; }
        public double VCpi { get; private set; }
        public double VCpo { get; private set; }
        public double VSbm { get; private set; }
        public double VScm { get; private set; }
        public double VSem { get; private set; }
        public double VShb { get; private set; }
        public double VSmb { get; private set; }
        public double VSrm { get; private set; }
        public double VAp { get; private set; }
        public double VSp { get; private set; }
        public double VTbl { get; private set; }
        public double VTcr { get; private set; }
        public double VTdo { get; private set; }
        public double VTha { get; private set; }
        public double VThi { get; private set; }
        public double VTpa { get; private set; }
        public double VTre { get; private set; }
        public double Ap { get; private set; }
        public double Cai { get; private set; }
        public double Cao { get; private set; }
        public double Cdi { get; private set; }
        public double Cdo { get; private set; }
        public double Chi { get; private set; }
        public double Cho { get; private set; }
        public double Cmc { get; private set; }
        public double Cmi { get; private set; }
        public double Cmo { get; private set; }
        public double Cpi { get; private set; }
        public double Cpo { get; private set; }
        public double Sbm { get; private set; }
        public double Scm { get; private set; }
        public double Sem { get; private set; }
        public double Shb { get; private set; }
        public double Smb { get; private set; }
        public double Sp { get; private set; }
        public double Srm { get; private set; }
        public double Tbl { get; private set; }
        public double Tcr { get; private set; }
        public double Tdo { get; private set; }
        public double Tha { get; private set; }
        public double Thi { get; private set; }
        public double Tpa { get; private set; }
        public double Tre { get; private set; }
        public int MinDmg { get; private set; }
        public int MaxDmg { get; private set; }

        public void Update()
        {
            InitInnateStats();
            ApplyCoreStatRatings();
            ApplyAuraEffects();
            InitDamage();
        }

        private void ApplyAuraEffects()
        {
            foreach (var effect in Effects)
                if (effect.Stat == "tha")
                    switch (effect.Type)
                    {
                        case "+":
                        {
                            VTha += effect.Value;
                            break;
                        }
                        case "-":
                        {
                            VTha -= effect.Value;
                            break;
                        }
                        default:
                        {
                            VTha *= effect.Value;
                            break;
                        }
                    }
                else if (effect.Stat == "tdo")
                    switch (effect.Type)
                    {
                        case "+":
                        {
                            VTdo += effect.Value;
                            break;
                        }
                        case "-":
                        {
                            VTdo -= effect.Value;
                            break;
                        }
                        default:
                        {
                            VTdo *= effect.Value;
                            break;
                        }
                    }
                else if (effect.Stat == "thi")
                    switch (effect.Type)
                    {
                        case "+":
                            VThi += effect.Value;
                            break;
                        case "-":
                            VThi -= effect.Value;
                            break;
                        default:
                            VThi *= effect.Value;
                            break;
                    }
                else if (effect.Stat == "tcr")
                    switch (effect.Type)
                    {
                        case "+":
                            VTcr += effect.Value;
                            break;
                        case "-":
                            VTcr -= effect.Value;
                            break;
                        default:
                            VTcr *= effect.Value;
                            break;
                    }
        }

        private void InitInnateStats()
        {
            var userClass = User.UserClass;
            var level = User.Character.Level;
            var cat = userClass.ItemModel != null
                ? (userClass.ItemModel.ItemClass != null ? userClass.ItemModel.ItemClass.Category : "M1")
                : "M1";
            var innateStat = ServerCoreValues.GetInnateStats(level);
            var ratios = Stats.GetClassCatList(cat);
            foreach (var key in Stats.GetStats()) Innate[key] = Convert.ToInt32(Math.Round(ratios[key] * innateStat));
        }

        private void ApplyCoreStatRatings()
        {
            var userClass = User.UserClass;
            var cat = userClass.ItemModel.ItemClass != null ? userClass.ItemModel.ItemClass.Category : "M1";
            var enhancement = WeaponEnhancementModel;
            var level = User.Character.Level;
            var wLvl = enhancement?.Level ?? 1.0D;
            var iDps = enhancement?.Dps ?? 100.0D;
            iDps = Math.Abs(iDps) < 0.0D ? 100.0D : iDps;
            iDps /= 100.0D;
            var intAPtoDps = ServerCoreValues.Get("intAPtoDPS");
            var pcdpsMod = ServerCoreValues.Get("PCDPSMod");
            var hpTgt = ServerCoreValues.GetBaseHPByLevel(level);
            const double ttd = 20.0D;
            var tDps = hpTgt / 20.0D * 0.7D;
            var sp1Pc = 2.25D * tDps / (100.0D / intAPtoDps) / 2.0D;
            ResetValues();
            foreach (var key in Innate.Keys)
            {
                double val = Innate[key] + Armor[key] + Weapon[key] + Helm[key] + Cape[key];
                switch (key)
                {
                    case "STR":
                        if (cat.Equals("M1")) VSbm -= val / sp1Pc / 100.0D * 0.3D;
                        if (cat.Equals("S1"))
                            VAp += Math.Round(val * 1.4D);
                        else
                            VAp += val * 2.0D;
                        if (!cat.Equals("M1") && !cat.Equals("M2") && !cat.Equals("M3") && !cat.Equals("M4") &&
                            !cat.Equals("S1")) continue;
                        if (cat.Equals("M4"))
                            VTcr += val / sp1Pc / 100.0D * 0.7D;
                        else
                            VTcr += val / sp1Pc / 100.0D * 0.4D;
                        break;
                    case "INT":
                        VCmi -= val / sp1Pc / 100.0D;
                        if (cat.Substring(0, 1).Equals("C") || cat.Equals("M3")) VCmo += val / sp1Pc / 100.0D;
                        if (cat.Equals("S1"))
                            VSp += Math.Round(val * 1.4D);
                        else
                            VSp += val * 2.0D;
                        if (!cat.Equals("C1") && !cat.Equals("C2") && !cat.Equals("C3") && !cat.Equals("M3") &&
                            !cat.Equals("S1")) continue;
                        if (cat.Equals("C2"))
                            VTha += val / sp1Pc / 100.0D * 0.5D;
                        else
                            VTha += val / sp1Pc / 100.0D * 0.3D;
                        break;
                    case "DEX":
                        if (cat.Equals("M1") || cat.Equals("M2") || cat.Equals("M3") || cat.Equals("M4") ||
                            cat.Equals("S1"))
                        {
                            if (!cat.Substring(0, 1).Equals("C")) VThi += val / sp1Pc / 100.0D * 0.2D;
                            if (!cat.Equals("M2") && !cat.Equals("M4"))
                                VTha += val / sp1Pc / 100.0D * 0.3D;
                            else
                                VTha += val / sp1Pc / 100.0D * 0.5D;
                            if (cat.Equals("M1") && Tbl > 0.01D) VTbl += val / sp1Pc / 100.0D * 0.5D;
                        }

                        if (!cat.Equals("M2") && !cat.Equals("M3"))
                            VTdo += val / sp1Pc / 100.0D * 0.3D;
                        else
                            VTdo += val / sp1Pc / 100.0D * 0.5D;
                        break;
                    case "WIS":
                        if (cat.Equals("C1") || cat.Equals("C2") || cat.Equals("C3") || cat.Equals("S1"))
                        {
                            if (cat.Equals("C1"))
                                VTcr += val / sp1Pc / 100.0D * 0.7D;
                            else
                                VTcr += val / sp1Pc / 100.0D * 0.4D;

                            VThi += val / sp1Pc / 100.0D * 0.2D;
                        }

                        VTdo += val / sp1Pc / 100.0D * 0.3D;
                        break;
                    case "LCK":
                        VSem += val / sp1Pc / 100.0D * 2.0D;
                        if (cat.Equals("S1"))
                        {
                            VAp += Math.Round(val * 1.0D);
                            VSp += Math.Round(val * 1.0D);
                            VTcr += val / sp1Pc / 100.0D * 0.3D;
                            VThi += val / sp1Pc / 100.0D * 0.1D;
                            VTha += val / sp1Pc / 100.0D * 0.3D;
                            VTdo += val / sp1Pc / 100.0D * 0.25D;
                            VScm += val / sp1Pc / 100.0D * 2.5D;
                        }
                        else
                        {
                            if (cat.Equals("M1") || cat.Equals("M2") || cat.Equals("M3") || cat.Equals("M4"))
                                VAp += Math.Round(val * 0.7D);
                            if (cat.Equals("C1") || cat.Equals("C2") || cat.Equals("C3") || cat.Equals("M3"))
                                VSp += Math.Round(val * 0.7D);
                            VTcr += val / sp1Pc / 100.0D * 0.2D;
                            VThi += val / sp1Pc / 100.0D * 0.1D;
                            VTha += val / sp1Pc / 100.0D * 0.1D;
                            VTdo += val / sp1Pc / 100.0D * 0.1D;
                            VScm += val / sp1Pc / 100.0D * 5.0D;
                        }

                        break;
                }
            }

            WDps = (int) (Math.Round(ServerCoreValues.GetBaseHPByLevel((int) wLvl) / ttd * iDps * pcdpsMod) +
                          Math.Round(VAp / intAPtoDps));
            MDps = (int) (Math.Round(ServerCoreValues.GetBaseHPByLevel((int) wLvl) / ttd * iDps * pcdpsMod) +
                          Math.Round(VSp / intAPtoDps));
        }

        private void InitDamage()
        {
            if (WeaponModel == null || !User.Skills.TryGetValue("aa", out var skillModel)) return;
            const double wSpd = 2.0D;
            var wDmg = WDps * wSpd;
            double wepRng = WeaponModel.Range;
            var iRng = wepRng / 100.0D;
            var tDmg = wDmg * skillModel.Damage;
            MinDmg = (int) Math.Floor(tDmg - tDmg * iRng);
            MaxDmg = (int) Math.Ceiling(tDmg + tDmg * iRng);
        }

        private void ResetValues()
        {
            Ap = 0.0D;
            VAp = 0.0D;
            Sp = 0.0D;
            VSp = 0.0D;
            Tbl = 0.0D;
            Tpa = 0.0D;
            Tdo = 0.0D;
            Tcr = 0.0D;
            Thi = 0.0D;
            Tha = 0.0D;
            Tre = 0.0D;
            VTbl = ServerCoreValues.Get(Indent.Core.BaseBlock);
            VTpa = ServerCoreValues.Get(Indent.Core.BaseParry);
            VTdo = ServerCoreValues.Get(Indent.Core.BaseDodge);
            VTcr = ServerCoreValues.Get(Indent.Core.BaseCrit);
            VThi = ServerCoreValues.Get(Indent.Core.BaseHit);
            VTha = ServerCoreValues.Get(Indent.Core.BaseHaste);
            VTre = 0.0D;
            Cpo = 1.0D;
            Cpi = 1.0D;
            Cao = 1.0D;
            Cai = 1.0D;
            Cmo = 1.0D;
            Cmi = 1.0D;
            Cdo = 1.0D;
            Cdi = 1.0D;
            Cho = 1.0D;
            Chi = 1.0D;
            Cmc = 1.0D;
            VCpo = 1.0D;
            VCpi = 1.0D;
            VCao = 1.0D;
            VCai = 1.0D;
            VCmo = 1.0D;
            VCmi = 1.0D;
            VCdo = 1.0D;
            VCdi = 1.0D;
            VCho = 1.0D;
            VChi = 1.0D;
            VCmc = 1.0D;
            Scm = ServerCoreValues.Get(Indent.Core.BaseCritValue);
            Sbm = ServerCoreValues.Get(Indent.Core.BaseBlockValue);
            Srm = ServerCoreValues.Get(Indent.Core.BaseResistValue);
            Sem = ServerCoreValues.Get(Indent.Core.BaseEventValue);
            VScm = ServerCoreValues.Get(Indent.Core.BaseCritValue);
            VSbm = ServerCoreValues.Get(Indent.Core.BaseBlockValue);
            VSrm = ServerCoreValues.Get(Indent.Core.BaseResistValue);
            VSem = ServerCoreValues.Get(Indent.Core.BaseEventValue);
            Shb = 0.0D;
            Smb = 0.0D;
            VShb = 0.0D;
            VSmb = 0.0D;
        }

        public int Get_V_DEX()
        {
            return Weapon["DEX"] + Armor["DEX"] + Helm["DEX"] + Cape["DEX"];
        }

        public int Get_V_END()
        {
            return Weapon["END"] + Armor["END"] + Helm["END"] + Cape["END"];
        }

        public int Get_V_INT()
        {
            return Weapon["INT"] + Armor["INT"] + Helm["INT"] + Cape["INT"];
        }

        public int Get_V_LCK()
        {
            return Weapon["LCK"] + Armor["LCK"] + Helm["LCK"] + Cape["LCK"];
        }

        public int Get_V_STR()
        {
            return Weapon["STR"] + Armor["STR"] + Helm["STR"] + Cape["STR"];
        }

        public int Get_V_WIS()
        {
            return Weapon["WIS"] + Armor["WIS"] + Helm["WIS"] + Cape["WIS"];
        }

        public int Get_DEX()
        {
            return Innate["DEX"];
        }

        public int Get_END()
        {
            return Innate["END"];
        }

        public int Get_INT()
        {
            return Innate["INT"];
        }

        public int Get_LCK()
        {
            return Innate["LCK"];
        }

        public int Get_STR()
        {
            return Innate["STR"];
        }

        public int Get_WIS()
        {
            return Innate["WIS"];
        }
    }
}