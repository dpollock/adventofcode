using System;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2024.Day02
{
  [ProblemName("Red-Nosed Reports")]
  class Solution : ISolver
  {
    public (long, long) Solve(string input)
    {
      var data = input.ReadLines(line => line.Split(' ').Select(int.Parse).ToArray());

      var part1 = data.Count(d => IsSafe(d, false) || IsSafe(d.Reverse().ToArray(), false));
      var part2 = data.Count(d => IsSafe(d, true) || IsSafe(d.Reverse().ToArray(), true));

      return (part1, part2);
    }

    public (long, long, long, long) SolveSample()
    {
      var data =
        @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9".ReadLines(line => line.Split(' ').Select(int.Parse).ToArray());

      var part1 = data.Count(d => IsSafe(d, false) || IsSafe(d.Reverse().ToArray(), false));
      var part2 = data.Count(d => IsSafe(d, true) || IsSafe(d.Reverse().ToArray(), true));

      return (part1, part2, 2, 4);
    }

    private bool IsSafe(int[] d, bool canSkip)
    {
      for (int i = 0; i < d.Length - 1; i++)
      {
        if (!(1 <= d[i] - d[i + 1] && d[i] - d[i + 1] <= 3))
        {
          if (canSkip)
          {
            // Try removing either current or next number
            return new[] { i, i + 1 }.Any(j =>
            {
              var newArray = new int[d.Length - 1];
              Array.Copy(d, 0, newArray, 0, j);
              Array.Copy(d, j + 1, newArray, j, d.Length - j - 1);
              return IsSafe(newArray, false);
            });
          }
          return false;
        }
      }
      return true;
    }

  }
}
