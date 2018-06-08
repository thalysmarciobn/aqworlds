using Newtonsoft.Json;

namespace JSON.Model
{
    public sealed class JsonStuSta
    {
        public int _INT { get; set; }
        public int _STR { get; set; }
        public int _DEX { get; set; }
        public int _END { get; set; }
        public int _LCK { get; set; }
        public int _WIS { get; set; }

        [JsonProperty("$INT")] public int V_INT { get; set; }

        [JsonProperty("$STR")] public int V_STR { get; set; }

        [JsonProperty("$DEX")] public int V_DEX { get; set; }

        [JsonProperty("$END")] public int V_END { get; set; }

        [JsonProperty("$LCK")] public int V_LCK { get; set; }

        [JsonProperty("$WIS")] public int V_WIS { get; set; }

        public int minDmg { get; set; }
        public int maxDmg { get; set; }
        public double _tha { get; set; }
        public double _srm { get; set; }
        public double _tcr { get; set; }
        public double _tpa { get; set; }
        public double _sbm { get; set; }
        public double _thi { get; set; }
        public double _cai { get; set; }
        public double _smb { get; set; }
        public double _cmo { get; set; }
        public double _cmi { get; set; }
        public double _sem { get; set; }
        public double _cao { get; set; }
        public double _shb { get; set; }
        public double _cdo { get; set; }
        public double _tre { get; set; }
        public double _cdi { get; set; }
        public double _cho { get; set; }
        public double _chi { get; set; }
        public double _ap { get; set; }
        public double _cpo { get; set; }
        public double _tbl { get; set; }
        public double _cmc { get; set; }
        public double _tdo { get; set; }
        public double _cpi { get; set; }
        public double _scm { get; set; }
        public double _sp { get; set; }

        [JsonProperty("$tha")] public double v_tha { get; set; }

        [JsonProperty("$srm")] public double v_srm { get; set; }

        [JsonProperty("$tcr")] public double v_tcr { get; set; }

        [JsonProperty("$tpa")] public double v_tpa { get; set; }

        [JsonProperty("$sbm")] public double v_sbm { get; set; }

        [JsonProperty("$thi")] public double v_thi { get; set; }

        [JsonProperty("$cai")] public double v_cai { get; set; }

        [JsonProperty("$smb")] public double v_smb { get; set; }

        [JsonProperty("$cmo")] public double v_cmo { get; set; }

        [JsonProperty("$cmi")] public double v_cmi { get; set; }

        [JsonProperty("$sem")] public double v_sem { get; set; }

        [JsonProperty("$cao")] public double v_cao { get; set; }

        [JsonProperty("$shb")] public double v_shb { get; set; }

        [JsonProperty("$cdo")] public double v_cdo { get; set; }

        [JsonProperty("$tre")] public double v_tre { get; set; }

        [JsonProperty("$cdi")] public double v_cdi { get; set; }

        [JsonProperty("$cho")] public double v_cho { get; set; }

        [JsonProperty("$chi")] public double v_chi { get; set; }

        [JsonProperty("$ap")] public double v_ap { get; set; }

        [JsonProperty("$cpo")] public double v_cpo { get; set; }

        [JsonProperty("$tbl")] public double v_tbl { get; set; }

        [JsonProperty("$cmc")] public double v_cmc { get; set; }

        [JsonProperty("$tdo")] public double v_tdo { get; set; }

        [JsonProperty("$cpi")] public double v_cpi { get; set; }

        [JsonProperty("$scm")] public double v_scm { get; set; }

        [JsonProperty("$sp")] public double v_sp { get; set; }
    }
}