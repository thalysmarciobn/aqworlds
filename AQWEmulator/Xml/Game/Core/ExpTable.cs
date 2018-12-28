using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game.Core
{
    [Serializable]
    public class ExpTable
    {
        [XmlElement("Levels")]
        public ExpTableData[] Levels { get; set; }

        public ExpTable()
        {
            Levels = LoadCoreLevels(Path.Combine(Application.StartupPath, @"data/cache/ServerLevel.xml"),
                "level", "rate", "value");
        }

        private static ExpTableData[] LoadCoreLevels(string path, string level, string rate, string value)
        {
            return (from core in XmlUtils.Values(path, value) where core.Attribute(level) != null && core.Attribute(rate) != null select new ExpTableData() {Level = Convert.ToInt32(core.Attribute(level)?.Value), Experience = Convert.ToInt32(core.Value), Rate = 1}).ToArray();
        }
    }
}