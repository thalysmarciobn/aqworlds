using System;

namespace AQWEmulator.Combat
{
    public class DamageEval
    {
        public DamageEval(string element, string tgtElement, int maxDamage, int minDamage, double multiplier,
            double critChance, double dodgeChance, double missChance, double critValue)
        {
            _critValue = critValue;
            _crit = Random.NextDouble() < critChance;
            _dodge = Random.NextDouble() < dodgeChance;
            _miss = Random.NextDouble() < missChance;
            _evalDamage = (int) ((Random.Next(maxDamage - minDamage) + minDamage) * multiplier);
        }

        private readonly double _critValue;
        private readonly bool _miss;
        private readonly bool _dodge;
        private readonly bool _crit;
        private readonly int _evalDamage;
        private static Random Random => new Random();

        public string Type => _evalDamage > 0
            ? _crit ? DamageType.Critical :
            _dodge ? DamageType.Dodge :
            _miss ? DamageType.Miss : DamageType.Hit
            : DamageType.None;

        public int Damage => _miss || _dodge ? 0 : _crit ? (int) (_evalDamage * _critValue) : _evalDamage;
    }
}