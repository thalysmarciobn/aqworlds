namespace JSON.Command
{
    public sealed class JsonEquipItem : IJsonPacket
    {
        public JsonEquipItem()
        {
            cmd = "equipItem";
        }

        public string cmd { get; }
        public string strES { get; set; }
        public string sFile { get; set; }
        public string sLink { get; set; }
        public string sMeta { get; set; }
        public string sType { get; set; }
        public int uid { get; set; }
        public int ItemID { get; set; }
    }
}