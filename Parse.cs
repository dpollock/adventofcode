using System.Text.RegularExpressions;

namespace AdventOfCode;

// examples of usage:
//   // Basic
//   var lines = Parse.Lines(input);
//   var nums = Parse.Ints(input);

//   // Custom parser per line
//   var moves = Parse.Lines(input, line => (line[0], int.Parse(line[1..])));

//   // Regex with mapper
//   var data = Parse.Regex(input, @"(\w+) (\d+)", m => new {
//       Dir = m.Groups[1].Value,
//       Dist = int.Parse(m.Groups[2].Value)
//   });

//   // Grids
//   var grid = Parse.Grid(input);       // char[][]
//   var nums = Parse.IntGrid(input);    // int[][] (single digits)

//   // Grouped input (blank line separated)
//   var groups = Parse.Groups(input);
public static class Parse
{
    public static string[] Lines(string input) => input.Split('\n');

    public static IEnumerable<T> Regex<T>(string input, string pattern, Func<Match, T> mapper, string separator = "\n")
    {
        var regex = new Regex(pattern);
        foreach (var line in input.Split(separator))
        {
            var match = regex.Match(line);
            if (match.Success)
                yield return mapper(match);
        }
    }

    public static char[][] Grid(string input) =>
        Lines(input).Select(l => l.ToCharArray()).ToArray();

    public static int[][] IntGrid(string input) =>
        Lines(input).Select(l => l.Select(c => c - '0').ToArray()).ToArray();

    public static List<List<string>> Groups(string input, string separator = "") =>
        input.Split(separator == "" ? "\n\n" : separator)
            .Select(g => Lines(g).ToList())
            .ToList();
}
