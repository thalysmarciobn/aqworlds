using System.Collections.Generic;

namespace AQWEmulator.Database.Models
{
    public class ShopModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Field { get; protected set; }
        public virtual int House { get; protected set; }
        public virtual int Upgrade { get; protected set; }
        public virtual int Staff { get; protected set; }
        public virtual int Limited { get; protected set; }
        public virtual IList<ShopItemModel> Items { get; protected set; }
    }
}