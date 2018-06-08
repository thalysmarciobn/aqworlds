using System;

namespace AQWEmulator.Database.Models
{
    public class UserModel : IModel
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual DateTime LastAccess { get; set; }
    }
}