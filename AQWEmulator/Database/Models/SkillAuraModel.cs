namespace AQWEmulator.Database.Models
{
    public class SkillAuraModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int SkillId { get; protected set; }
        public virtual int AuraId { get; protected set; }
    }
}