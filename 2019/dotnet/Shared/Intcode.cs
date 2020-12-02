using System.IO;

namespace Shared
{
    public static class Intcode
    {
        public static int[] ParseFile(string fileName)
        {
            var strVals = File.ReadAllText(fileName).Split(",");
            var intCodes = new int[strVals.Length];
            for (var i = 0; i < strVals.Length; i++)
            {
                intCodes[i] = int.Parse(strVals[i]);
            }
            return intCodes;
        }
    }
}