namespace AQWEmulator.Combat
{
    public abstract class AbstractState
    {
        protected AbstractState(int maxHealth, int maxMana)
        {
            Health = maxHealth;
            Mana = maxMana;
            MaxHealth = maxHealth;
            MaxMana = maxMana;
            State = 1;
        }

        public int MaxHealth { get; private set; }
        public int MaxMana { get; private set; }
        public int Health { get; private set; }
        public int Mana { get; private set; }
        public int State { get; private set; }

        public bool IsDead => State == 0 || Health == 0;

        public bool IsNeutral => State == 1;

        public bool OnCombat => State == 2;

        public void SetHealth(int health)
        {
            if (health <= 0)
                Die();
            else if (health > MaxHealth)
                Health = MaxHealth;
            else
                Health = health;
        }

        public void SetMana(int mana)
        {
            if (mana < 0)
                Mana = 0;
            else if (mana > MaxMana)
                Mana = MaxMana;
            else
                Mana = mana;
        }

        public void DecreaseHealth(int health)
        {
            SetHealth(Health - health);
        }

        public void IncreaseHealth(int health)
        {
            SetHealth(Health + health);
        }

        public void DecreaseMana(int mana)
        {
            SetMana(Mana - mana);
        }

        public void IncreaseMana(int mana)
        {
            SetMana(Mana + mana);
        }

        public void DecreaseHealthByPercent(double percent)
        {
            var amount = (int) (MaxHealth * percent);
            SetHealth(Health - amount);
        }

        public void IncreaseHealthByPercent(double percent)
        {
            var amount = (int) (MaxHealth * percent);
            SetHealth(Health + amount);
        }

        public void DecreaseManaByPercent(double percent)
        {
            var amount = (int) (MaxMana * percent);
            SetMana(Mana - amount);
        }

        public void IncreaseManaByPercent(double percent)
        {
            var amount = (int) (MaxMana * percent);
            SetMana(Mana + amount);
        }

        protected virtual void Die()
        {
            Health = 0;
            Mana = 0;
            State = 0;
        }

        public void Restore()
        {
            SetState(1);
            SetHealth(MaxHealth);
            SetMana(MaxMana);
        }

        public virtual void SetState(int state)
        {
            if (state >= 0 && state <= 2) State = state;
        }

        public void SetMaxHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
        }

        public void SetMaxMana(int maxMana)
        {
            MaxMana = maxMana;
        }
    }
}