namespace JSON.Command
{
    public sealed class JsonLoginResponse : IJsonPacket
    {
        public string Cmd { get; } = "loginResponse";
        public bool Success { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public string serverTime { get; set; }
        public string info { get; set; }
        public string MOTD { get; set; }
    }
}