using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace adventofcode.Lib;

public static class AoCHelpers
{
    public static IEnumerable<T> ReadLinesToType<T>(this string s) where T : IConvertible
    {
        using var sr = new StringReader(s);
        while (sr.ReadLine() is { } line)
            yield return (T)Convert.ChangeType(line, typeof(T));
    }

    public static IEnumerable<List<T>> ReadLinesToGroupsOfType<T>(this string s) where T : IConvertible
    {
        var result = new List<List<T>>();
        var currentGroup = new List<T>();
        using var sr = new StringReader(s);
        while (sr.ReadLine() is { } line)
        {
            if (string.IsNullOrEmpty(line))
            {
                result.Add(currentGroup);
                currentGroup = new List<T>();
            }
            else
            {
                currentGroup.Add((T)Convert.ChangeType(line, typeof(T)));
            }
        }

        return result;
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