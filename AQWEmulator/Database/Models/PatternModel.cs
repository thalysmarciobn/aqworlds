namespace AQWEmulator.Database.Models
{
    public class PatternModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual int Wisdom { get; protected set; }
        public virtual int Strength { get; protected set; }
        public virtual int Luck { get; protected set; }
        public virtual int Dexterity { get; protected set; }
        public virtual int Endurance { get; protected set; }
        public virtual int Intelligence { get; protected set; }
    }
}