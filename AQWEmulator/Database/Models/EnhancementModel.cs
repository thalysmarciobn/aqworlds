namespace AQWEmulator.Database.Models
{
    public class EnhancementModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual int PatternId { get; protected set; }
        public virtual int Rarity { get; protected set; }
        public virtual int Dps { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual PatternModel Pattern { get; protected set; }
    }
}