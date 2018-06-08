using System.Collections.Generic;
using System.Linq;

namespace Utils.AQW
{
    public static class SmartFoxServer
    {
        public static string Parse(IEnumerable<string> data)
        {
            return data.Aggregate("%", (current, e) => current + e + "%");
        }
    }
}