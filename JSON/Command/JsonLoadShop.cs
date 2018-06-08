using JSON.Model;

namespace JSON.Command
{
    public sealed class JsonLoadShop : IJsonPacket
    {
        public JsonLoadShop()
        {
            cmd = "loadShop";
        }

        public string cmd { get; }

        public JsonShopInfo shopinfo { get; set; }
    }
}