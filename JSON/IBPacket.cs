namespace JSON
{
    public interface IBPacket
    {
        int r { get; set; }
        IJsonPacket o { get; set; }
    }
}