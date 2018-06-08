using System.Collections.Generic;

namespace AQWEmulator.Database.Models
{
    public class ItemClassModel : IModel
    {
        public virtual int ItemId { get; protected set; }
        public virtual string Category { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string ManaRegenerationMethods { get; protected set; }
        public virtual string StatsDescription { get; protected set; }
        public virtual IList<ClassSkillModel> Skills { get; protected set; }
    }
}