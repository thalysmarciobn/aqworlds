namespace AQWEmulator.Database.Models
{
    public class ItemModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string Type { get; protected set; }
        public virtual string Element { get; protected set; }
        public virtual string File { get; protected set; }
        public virtual string Link { get; protected set; }
        public virtual string Icon { get; protected set; }
        public virtual string Equipment { get; protected set; }
        public virtual int Level { get; protected set; }
        public virtual int Dps { get; protected set; }
        public virtual int Range { get; protected set; }
        public virtual int Rarity { get; protected set; }
        public virtual int Quantity { get; protected set; }
        public virtual int Stack { get; protected set; }
        public virtual int Cost { get; protected set; }
        public virtual int Coins { get; protected set; }
        public virtual int Sellable { get; protected set; }
        public virtual int Temporary { get; protected set; }
        public virtual int Upgrade { get; protected set; }
        public virtual int Staff { get; protected set; }
        public virtual int EnhId { get; protected set; }
        public virtual int FactionId { get; protected set; }
        public virtual int ReqReputation { get; protected set; }
        public virtual int ReqClassId { get; protected set; }
        public virtual int ReqClassPoints { get; protected set; }
        public virtual string ReqQuests { get; protected set; }
        public virtual int QuestStringIndex { get; protected set; }
        public virtual int QuestStringValue { get; protected set; }
        public virtual string Meta { get; protected set; }
        public virtual ItemClassModel ItemClass { get; protected set; }
        public virtual EnhancementModel Enhancement { get; protected set; }
    }
}