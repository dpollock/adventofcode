using System;
using System.Collections.Generic;
using System.IO;

namespace adventofcode.Lib
{
    public static class AoCHelpers
    {
        public static IEnumerable<T> ReadLines<T>(this string s) where T : IConvertible
        {
            string line;
            using var sr = new StringReader(s);
            while ((line = sr.ReadLine()) != null)
                yield return (T)Convert.ChangeType(line, typeof(T));
        }
    }
}