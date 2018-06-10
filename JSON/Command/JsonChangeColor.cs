namespace JSON.Command
{
    public class JsonChangeColor : IJsonPacket
    {
        public JsonChangeColor()
        {
            cmd = "changeColor";
        }
        
        public string cmd { get; }
        
        public int uid { get; set; }
        
        public int HairID { get; set; }
        
        public int intColorSkin { get; set; }
        
        public int intColorHair { get; set; }
        
        public int intColorEye { get; set; }
        
        public string strHairName { get; set; }
        
        public string strHairFilename { get; set; }
    }
}