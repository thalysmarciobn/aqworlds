using System.Collections.Generic;
using System.Linq;

namespace AQWEmulator.Utils
{
    public static class SmartFoxServer
    {
        public static string Parse(IEnumerable<string> data)
        {
            return data.Aggregate("%", (current, e) => current + e + "%");
        }
    }
}