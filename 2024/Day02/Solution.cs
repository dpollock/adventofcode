using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2024.Day02
{
  [ProblemName("Red-Nosed Reports")]
  class Solution : ISolver
  {

    public (long, long) Solve(string input)
    {
      var lines = input.ReadLines(line => line.Split(' ').Select(int.Parse).ToList());

      var part1 = CalculatePart1(lines);
      var part2 = CalculatePart2(lines);

      return (part1, part2);
    }


    public (long, long, long, long) SolveSample()
    {
      var lines =
        @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9".ReadLines(line => line.Split(' ').Select(int.Parse).ToList());
      var part1 = CalculatePart1(lines);
      var part2 = CalculatePart2(lines);

      return (part1, part2, 2, 4);
    }

    private long CalculatePart1(IEnumerable<List<int>> input)
    {
      return input.Count(levels => IsSafe(levels, -1));
    }


    private long CalculatePart2(IEnumerable<List<int>> input)
    {
      return input.Count(levels =>
          Enumerable.Range(-1, levels.Count + 1)
                   .Any(index => IsSafe(levels.ToList(), index)));
    }

    private static bool IsSafe(List<int> levels, int skipIndex)
    {
      var sequence = skipIndex >= 0 && skipIndex < levels.Count
          ? levels.Where((_, i) => i != skipIndex)
          : levels;

      var differences = sequence
          .Zip(sequence.Skip(1), (a, b) => b - a)
          .ToList();

      if (differences.Count == 0 || differences[0] == 0)
        return false;

      return differences.All(diff => CheckIsValid(diff, Math.Sign(differences[0])));
    }

    private static bool CheckIsValid(int difference, int sign)
    {
      return Math.Abs(difference) is >= 1 and <= 3 && Math.Sign(difference) == sign;
    }
  }
}
