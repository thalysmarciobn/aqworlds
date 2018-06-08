using System;

namespace AQWEmulator.Database.Models
{
    public class CharacterItemModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual int UserId { get; set; }
        public virtual int CharacterId { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int EnhId { get; set; }
        public virtual int Equipped { get; set; }
        public virtual int Quantity { get; set; }
        public virtual int Bank { get; set; }
        public virtual DateTime DatePurchased { get; set; }
        public virtual ItemModel Item { get; set; }
        public virtual EnhancementModel Enhancement { get; set; }
    }
}