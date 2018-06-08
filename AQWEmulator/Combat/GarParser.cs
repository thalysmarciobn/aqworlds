using System.Collections.Generic;
using System.Text;

namespace AQWEmulator.Combat
{
    public static class GarParser
    {
        public static string ParseTargetInfo(string str)
        {
            var tb = new StringBuilder();
            if (str.Contains(","))
            {
                var multi = str.Split(',');
                for (var i = 0; i < multi.Length; ++i)
                {
                    if (i != 0) tb.Append(',');
                    tb.Append(multi[i].Split('>')[1]);
                }
            }
            else
            {
                tb.Append(str.Split('>')[1]);
            }

            return tb.ToString();
        }

        public static bool HasDuplicate(IEnumerable<string> targets)
        {
            IList<string> inputList = new List<string>(targets);
            var inputSet = new HashSet<string>(inputList);
            return inputSet.Count < inputList.Count;
        }

        public static string GetSkillReference(string str)
        {
            return str.Contains(",") ? str.Split(',')[0].Split('>')[0] : str.Split('>')[0];
        }

        public static bool IsSkillValid(string reference, int rank)
        {
            return (rank >= 2 || !reference.Equals("a2")) && (rank >= 3 || !reference.Equals("a3")) &&
                   (rank >= 5 || !reference.Equals("a4"));
        }
    }
}