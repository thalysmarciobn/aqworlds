namespace JSON.Command
{
    public sealed class JsonPartyInvite : IJsonPacket
    {
        public string cmd { get; } = "pi";
        public string owner { get; set; }
        public long pid { get; set; }
    }
}