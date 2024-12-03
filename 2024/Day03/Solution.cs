using System;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2024.Day03
{
  [ProblemName("Mull It Over")]
  class Solution : ISolver
  {
    private static readonly Regex MulPattern = new(@"mul\((\d+),(\d+)\)|do\(\)|don't\(\)", RegexOptions.Compiled);
    public (long, long) Solve(string input)
    {
      var part1 = 0L;
      var part2 = 0L;
      var doMul = true;
      foreach (Match match in MulPattern.Matches(input))
      {
        doMul = match.Value switch
        {
          "do()" => true,
          "don't()" => false,
          _ => doMul
        };

        if (match.Value.StartsWith("mul"))
        {
          var num1 = int.Parse(match.Groups[1].Value);
          var num2 = int.Parse(match.Groups[2].Value);
          var product = num1 * num2;
          part1 += product;
          if (doMul) part2 += product;
        }
      }
      return (part1, part2);
    }

    public (long, long, long, long) SolveSample()
    {
      var sample =
        @"xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

      var (part1, part2) = Solve(sample);
      return (part1, part2, 161, 48);
    }
  }
}
