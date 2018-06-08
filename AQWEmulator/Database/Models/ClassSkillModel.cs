namespace AQWEmulator.Database.Models
{
    public class ClassSkillModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int ItemId { get; protected set; }
        public virtual int SkillId { get; protected set; }
        public virtual SkillModel Skill { get; protected set; }
    }
}