using System.Collections.Generic;
using System.Linq;

namespace SupportBot
{
    public static class Extensions
    {
        public static IEnumerable<string> Split(this string str, int chunkSize)
        {
            if (str.Length <= chunkSize) return new[] { str };
            
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));

        }
    }
}