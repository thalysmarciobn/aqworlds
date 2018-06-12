namespace AQWEmulator.Database.Models
{
    public class AuraEffectModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int AuraId { get; protected set; }
        public virtual string Stat { get; protected set; }
        public virtual int Value { get; protected set; }
        public virtual string Type { get; protected set; }
    }
}