using System.Collections.Generic;

namespace AQWEmulator.Database.Models
{
    public class AuraModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual int Duration { get; protected set; }
        public virtual string Category { get; protected set; }
        public virtual double DamageIncrease { get; protected set; }
        public virtual double DamageTakenDecrease { get; protected set; }
        public virtual IList<AuraEffectModel> Effects { get; protected set; }
    }
}