using System.Threading.Tasks;
using System.Timers;

namespace AQWEmulator.Threading
{
    public static class ThreadHelper
    {
        public static void ExecuteWithDelay(AbstractThread task, int delay)
        {
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                task.Start();
            });
        }

        public static Timer Schedule(AbstractThread task, int delay)
        {
            var timer = new Timer {Interval = delay, Enabled = true};
            timer.Elapsed += (sender, e) => task.Start();
            return timer;
        }
    }
}