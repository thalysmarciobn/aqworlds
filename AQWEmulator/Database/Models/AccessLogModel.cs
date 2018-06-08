using System;

namespace AQWEmulator.Database.Models
{
    public class AccessLogModel : IModel
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
        public virtual int CharId { get; set; }
        public virtual string Address { get; set; }
        public virtual DateTime Date { get; set; }
    }
}