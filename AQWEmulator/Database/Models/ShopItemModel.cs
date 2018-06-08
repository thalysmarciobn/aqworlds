namespace AQWEmulator.Database.Models
{
    public class ShopItemModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int ShopId { get; protected set; }
        public virtual int ItemId { get; protected set; }
        public virtual int QuantityRemain { get; protected set; }
        public virtual ItemModel Item { get; protected set; }
    }
}