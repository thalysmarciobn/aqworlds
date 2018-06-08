namespace AQWEmulator.Database.Models
{
    public class AreaMonsterModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int AreaId { get; protected set; }
        public virtual int MonMapId { get; protected set; }
        public virtual int MonId { get; protected set; }
        public virtual string Frame { get; protected set; }
        public virtual MonsterModel Monster { get; protected set; }
    }
}