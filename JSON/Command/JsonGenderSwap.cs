namespace JSON.Command
{
    public class JsonGenderSwap : IJsonPacket
    {
        public JsonGenderSwap()
        {
            cmd = "genderSwap";
        }
        
        public string cmd { get; }
        
        public int uid { get; set; }
        
        public int HairID { get; set; }
        
        public int bitSuccess { get; set; }
        
        public int intCoins { get; set; }
        
        public string gender { get; set; }
        
        public string strHairName { get; set; }
        
        public string strHairFilename { get; set; }
    }
}