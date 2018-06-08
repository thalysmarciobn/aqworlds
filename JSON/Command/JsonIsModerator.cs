namespace JSON.Command
{
    public sealed class JsonIsModerator : IJsonPacket
    {
        public JsonIsModerator()
        {
            cmd = "isModerator";
        }

        public string cmd { get; }
        public string unm { get; set; }
        public bool val { get; set; }
    }
}