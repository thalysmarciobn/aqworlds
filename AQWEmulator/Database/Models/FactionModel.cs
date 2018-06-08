namespace AQWEmulator.Database.Models
{
    public class FactionModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
    }
}