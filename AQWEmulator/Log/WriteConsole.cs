using System;

namespace Utils.Log
{
    public static class WriteConsole
    {
        private static readonly object Lock = new object();

        public static void Info(string message)
        {
            lock (Lock)
            {
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }
    }
}