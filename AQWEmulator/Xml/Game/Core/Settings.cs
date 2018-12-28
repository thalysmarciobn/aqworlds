using System;
using System.Xml.Serialization;

namespace AQWEmulator.Xml.Game.Core
{
    [Serializable]
    public class Settings
    {
        [XmlElement("gMenu")]
        public string GMenu { get; set; }
        
        [XmlElement("sAssets")]
        public string SAssets { get; set; }
        
        [XmlElement("sBook")]
        public string SBook { get; set; }
        
        [XmlElement("sMap")]
        public string SMap { get; set; }
        
        [XmlElement("sNews")]
        public string SNews { get; set; }
        
        [XmlElement("sWTSandbox")]
        public bool SWtSandbox { get; set; }

        public Settings()
        {
            GMenu = "gMenu.swf";
            SAssets = "sAssets.swf";
            SBook = "sBook.swf";
            SMap = "sMap.swf";
            SNews = "sNews.swf";
            SWtSandbox = false;
        }
    }
}