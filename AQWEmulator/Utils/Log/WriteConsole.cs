using System;

namespace AQWEmulator.Utils.Log
{
    public static class WriteConsole
    {
        private static readonly object Lock = new object();

        public static void Info(string message)
        {
            lock (Lock)
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss"));
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" [INFO] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void Error(string message)
        {
            lock (Lock)
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss"));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" [ERROR] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        
        public static void Session(string message, string session)
        {
            lock (Lock)
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss"));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" [SESSION] ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"[{session}] ");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}