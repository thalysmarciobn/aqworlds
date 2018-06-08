namespace AQWEmulator.Database.Models
{
    public class SkillAuraEffectModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int AuraId { get; protected set; }
        public virtual string Stat { get; protected set; }
        public virtual double Value { get; protected set; }
        public virtual string Type { get; protected set; }
    }
}