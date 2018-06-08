using System.Collections.Generic;

namespace AQWEmulator.Database.Models
{
    public class AreaModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string File { get; protected set; }
        public virtual int MaxPlayers { get; protected set; }
        public virtual int ReqLevel { get; protected set; }
        public virtual int Upgrade { get; protected set; }
        public virtual int Staff { get; protected set; }
        public virtual int Pvp { get; protected set; }
        public virtual int Pk { get; protected set; }
        public virtual IList<AreaMonsterModel> AreaMonster { get; protected set; }
    }
}