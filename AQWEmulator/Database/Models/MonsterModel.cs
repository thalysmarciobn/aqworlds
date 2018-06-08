namespace AQWEmulator.Database.Models
{
    public class MonsterModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Race { get; protected set; }
        public virtual string File { get; protected set; }
        public virtual string Linkage { get; protected set; }
        public virtual string Element { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual int Heath { get; protected set; }
        public virtual int Mana { get; protected set; }
        public virtual int Gold { get; protected set; }
        public virtual int Coin { get; protected set; }
        public virtual int Experience { get; protected set; }
        public virtual int FactionId { get; protected set; }
        public virtual int Reputation { get; protected set; }
        public virtual int ClassPoint { get; protected set; }
        public virtual int TeamId { get; protected set; }
        public virtual int Respawn { get; protected set; }
        public virtual int Dps { get; protected set; }
        public virtual AreaMonsterModel AreaMonster { get; protected set; }
    }
}