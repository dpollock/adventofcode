using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day02;

[ProblemName("Cube Conundrum")]
internal partial class Solution : ISolver
{
    public object PartOne(string input)
    {
        var lines = input.ReadLines(ParseGame);
        var answer = lines.Where(x => x.MaxRed <= 12 && x.MaxGreen <= 13 && x.MaxBlue <= 14)
            .Sum(x => x.Id);

        return answer;
    }

    public object PartTwo(string input)
    {
        var lines = input.ReadLines(ParseGame);
        var answer = lines.Sum(x => x.MaxRed * x.MaxGreen * x.MaxBlue);
        return answer;
    }

    private CubeGame ParseGame(string s)
    {
        var lineSplit = s.Split(":");

        var gameIdMatch = GameRegex().Match(lineSplit[0]);
        var cubeMatches = CubeRegex().Matches(lineSplit[1]);

        var cubes = cubeMatches.Select(x => (Number: int.Parse(x.Groups[1].Value), Color: x.Groups[2].Value));

        var totals = cubes.Aggregate(new CubeGame { Id = int.Parse(gameIdMatch.Groups[1].Value) }, (agg, x) =>
            x.Color switch
            {
                "red" when x.Number > agg.MaxRed => agg with { MaxRed = x.Number },
                "blue" when x.Number > agg.MaxBlue => agg with { MaxBlue = x.Number },
                "green" when x.Number > agg.MaxGreen => agg with { MaxGreen = x.Number },
                _ => agg
            });

        return totals;
    }

    [GeneratedRegex(@"\w+\s(\d+)")]
    private static partial Regex GameRegex();

    [GeneratedRegex(@"(\d+) (\w+)")]
    private static partial Regex CubeRegex();

    public record CubeGame
    {
        public int Id { get; set; }
        public int MaxBlue { get; set; }
        public int MaxRed { get; set; }
        public int MaxGreen { get; set; }
    }

}