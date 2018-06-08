namespace AQWEmulator.World.Core
{
    public class CoreExpToLevel
    {
        public CoreExpToLevel(int level, double multi, int experience)
        {
            Level = level;
            Experience = experience;
            Multi = multi;
        }

        public int Level { get; }
        public int Experience { get; }
        public double Multi { get; }
    }
}