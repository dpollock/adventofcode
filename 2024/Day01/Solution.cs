using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2024.Day01
{
  [ProblemName("Historian Hysteria")]
  public class Solution : ISolver
  {
    private record InputLine
    {
      public int A { get; init; }
      public int B { get; init; }
    }

    public (long, long) Solve(string input)
    {
      var lines = input.ReadLinesToObject<InputLine>(fieldDelimiter: "   ");
      return (CalculatePart1(lines), CalculatePart2(lines));
    }

    public (long, long, long, long) SolveSample()
    {
      const string sampleInput =
        """
        3   4
        4   3
        2   5
        1   3
        3   9
        3   3
        """;

      var sample = sampleInput.ReadLinesToObject<InputLine>(fieldDelimiter: "   ");
      return (CalculatePart1(sample), CalculatePart2(sample), 11, 31);
    }

    private static long CalculatePart1(IEnumerable<InputLine> lines)
    {
      var listA = lines.Select(x => x.A).OrderBy(x => x).ToList();
      var listB = lines.Select(x => x.B).OrderBy(x => x).ToList();
      return listA.Zip(listB, (a, b) => Math.Abs(a - b)).Sum();
    }

    private static long CalculatePart2(IEnumerable<InputLine> lines)
    {
      var listA = lines.Select(x => x.A);
      var countOfEachNumberInB = lines

        .GroupBy(x => x.B)
        .ToDictionary(x => x.Key, x => x.Count());

      return listA.Sum(a => a * countOfEachNumberInB.GetValueOrDefault(a));
    }
  }
}
