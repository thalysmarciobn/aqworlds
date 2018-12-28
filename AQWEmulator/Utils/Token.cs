using System;
using System.Text;

namespace AQWEmulator.Utils
{
    public static class Token
    {
        public static string Create(int length)
        {
            var random = new Random();
            const string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++) result.Append(characters[random.Next(characters.Length)]);
            return result.ToString();
        }
    }
}