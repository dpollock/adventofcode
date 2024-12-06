using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2024.Day04
{
  [ProblemName("Ceres Search")]
  class Solution : ISolver
  {

    private static int[] N = [0, 1, 2, 3];

    public (long, long) Solve(string input)
    {
      var part1 = SolvePart1(input);
      var part2 = SolvePart2(input);

      return (part1, part2);
    }

    public int SolvePart1(string input)
    {
      string[] grid = input.ReadLines(line => line).ToArray();
      int count = 0;
      for (int y = 0; y < grid.Length; y++)
      {
        for (int x = 0; x < grid[0].Length; x++)
        {
          List<char[]> s = new();
          if (x <= grid[0].Length - 4)
            s.Add(N.Select(i => grid[y][x + i]).ToArray());
          if (y <= grid.Length - 4)
            s.Add(N.Select(i => grid[y + i][x]).ToArray());
          if (x <= grid[0].Length - 4 && y <= grid.Length - 4)
            s.Add(N.Select(i => grid[y + i][x + i]).ToArray());
          if (x >= 3 && y <= grid.Length - 4)
            s.Add(N.Select(i => grid[y + i][x - i]).ToArray());
          count += s.AsEnumerable().Count(t => t.SequenceEqual("XMAS") || t.SequenceEqual("SAMX"));
        }
      }
      return count;
    }

    public int SolvePart2(string input)
    {
      string[] grid = input.ReadLines(line => line).ToArray();
      int[] dx = new[] { 1, 1, -1, -1 };
      int[] dy = new[] { 1, -1, 1, -1 };
      int count = 0;
      for (int y = 1; y <= grid.Length - 2; y++)
      {
        for (int x = 1; x <= grid[0].Length - 2; x++)
        {
          if (grid[y][x] != 'A')
            continue;
          char[] nxts = N.Select(i => grid[y + dy[i]][x + dx[i]]).ToArray();
          if (nxts.All(n => n == 'M' || n == 'S') && nxts[0] != nxts[3] && nxts[1] != nxts[2])
            count += 1;
        }
      }
      return count;
    }

    public (long, long, long, long) SolveSample()
    {
      var sample =
        @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";
      var part1 = SolvePart1(sample);
      var part2 = SolvePart2(sample);

      return (part1, part2, 18, 0);
    }
  }
}
