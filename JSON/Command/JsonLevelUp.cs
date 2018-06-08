namespace JSON.Command
{
    public sealed class JsonLevelUp : IJsonPacket
    {
        public JsonLevelUp()
        {
            cmd = "levelUp";
        }

        public string cmd { get; }
        public int intLevel { get; set; }
        public int intExpToLevel { get; set; }
    }
}