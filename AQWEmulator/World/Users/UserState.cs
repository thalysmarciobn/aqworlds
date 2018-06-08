using System;
using System.Timers;
using AQWEmulator.Combat;
using AQWEmulator.Threading;
using AQWEmulator.Utils;
using AQWEmulator.World.Threads;

namespace AQWEmulator.World.Users
{
    public class UserState : AbstractState
    {
        public UserState(User user, int maxHealth, int maxMana) : base(maxHealth, maxMana)
        {
            User = user;
            Regeneration = ThreadHelper.Schedule(new UserRegeneration(User), 1500);
        }

        private User User { get; }
        private Timer Regeneration { get; }

        protected override void Die()
        {
            base.Die();
            //base.ClearAuras();
            User.ClearTargets();
            User.RoomUser.RespawnTime = DateTime.Now;
        }

        public override void SetState(int state)
        {
            base.SetState(state);
            if (state == Indent.State.IntNeutral)
                if (Health < MaxHealth || Mana < MaxMana)
                    Regeneration.Start();
        }

        public void SuspendRegenration()
        {
            Regeneration.Stop();
        }
    }
}