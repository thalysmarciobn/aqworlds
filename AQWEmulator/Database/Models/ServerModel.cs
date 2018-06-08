namespace AQWEmulator.Database.Models
{
    public class ServerModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual int Upgrade { get; protected set; }
        public virtual int Online { get; set; }
        public virtual int Chat { get; protected set; }
        public virtual int Max { get; protected set; }
        public virtual string Motd { get; protected set; }
        public virtual int Count { get; set; }
    }
}