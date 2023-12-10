using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day09;

[ProblemName("Mirage Maintenance")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var result = Part1(input);
        return result.part1;
    }

    public object PartTwo(string input)
    {
        var result = Part1(input);
        return result.part2;
    }

    private static (long part1, long part2) Part1(string input)
    {
        var S = input.ReadLinesToType<string>();
        long answer1 = 0;
        long answer2 = 0;

        foreach (var s in S)
        {
            var line = s.Split(" ").Select(long.Parse).ToList();
            var lines = new List<List<long>> { line };
            var allZeros = false;
            while (!allZeros)
            {
                line = new();
                var curLine = lines[^1];
                allZeros = true;
                for (var i = 0; i < curLine.Count - 1; i++)
                {
                    var diff = curLine[i + 1] - curLine[i];
                    if (diff != 0) allZeros = false;
                    line.Add(diff);
                }

                lines.Add(line);
            }

            answer1 += lines.Select(l => l[^1]).Sum();

            long tot = 0;
            for (var i = lines.Count - 1; i >= 0; i--) tot = lines[i][0] - tot;
            answer2 += tot;
        }


        return (answer1, answer2);
    }
}