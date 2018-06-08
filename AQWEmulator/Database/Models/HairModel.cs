namespace AQWEmulator.Database.Models
{
    public class HairModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string File { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Gender { get; protected set; }
    }
}