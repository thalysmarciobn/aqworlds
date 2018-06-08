namespace JSON
{
    public class BPacket : IBPacket
    {
        public int r { get; set; }
        public IJsonPacket o { get; set; }
    }
}