namespace AQWEmulator.Database.Models
{
    public class SkillModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Animation { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual double Damage { get; protected set; }
        public virtual int Mana { get; protected set; }
        public virtual string Icon { get; protected set; }
        public virtual int Range { get; protected set; }
        public virtual string Dsrc { get; protected set; }
        public virtual string Reference { get; protected set; }
        public virtual string Target { get; protected set; }
        public virtual string Effects { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual string Strl { get; protected set; }
        public virtual int Cooldown { get; protected set; }
        public virtual int HitTargets { get; protected set; }
        public virtual int AuraId { get; protected set; }
    }
}