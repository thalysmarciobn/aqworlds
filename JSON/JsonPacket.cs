namespace JSON
{
    public sealed class JsonPacket
    {
        public JsonPacket(int room, IJsonPacket packet)
        {
            b = new BPacket {r = room, o = packet};
        }

        public string t { get; } = "xt";
        public IBPacket b { get; }
    }
}