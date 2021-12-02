using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace adventofcode.Lib
{
    public static class AoCHelpers
    {
        public static IEnumerable<T> ReadLinesToType<T>(this string s) where T : IConvertible
        {
            string line;
            using var sr = new StringReader(s);
            while ((line = sr.ReadLine()) != null)
                yield return (T)Convert.ChangeType(line, typeof(T));
        } 

        public static IList<T> ReadLinesToObject<T>(this string s, string fieldDelimiter = " ")
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = fieldDelimiter,
                WhiteSpaceChars = Array.Empty<char>(),
                HasHeaderRecord = false,
                TrimOptions = TrimOptions.None,
                MissingFieldFound = args => { }
            };

            using var reader = new StringReader(s);
            using var csv = new CsvReader(reader, config);
            return csv.GetRecords<T>().ToList();
        }
    }
}