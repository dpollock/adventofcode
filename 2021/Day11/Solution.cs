using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day11
{
    [ProblemName("Dumbo Octopus")]
    internal class Solution : ISolver
    {
        private readonly Octopus[,] grid = new Octopus[10, 10];
        private readonly int sizei = 10;
        private readonly int sizej = 10;

        public object PartOne(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            for (var i = 0; i < sizei; i++)
                for (var j = 0; j < sizej; j++)
                    grid[i, j] = new Octopus(int.Parse(lines[i][j].ToString()));

            var total = 0;
            var steps = 100;
            for (var k = 1; k <= steps; k++)
            {
                var flashed = new List<Tuple<int, int>>();
                for (var i = 0; i < sizei; i++)
                    for (var j = 0; j < sizej; j++)
                        flashed.AddRange(Process(i, j));

                total += flashed.Count;
                foreach (var t in flashed) grid[t.Item1, t.Item2].Flashed = false;
            }

            return total;
        }

        public object PartTwo(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            for (var i = 0; i < sizei; i++)
                for (var j = 0; j < sizej; j++)
                    grid[i, j] = new Octopus(int.Parse(lines[i][j].ToString()));

            int step = 0;
            while (true)
            {
                step++;
                List<Tuple<int, int>> flashed = new List<Tuple<int, int>>();
                for (int i = 0; i < sizei; i++)
                {
                    for (int j = 0; j < sizej; j++)
                    {
                        flashed.AddRange(Process(i, j));
                    }
                }
                if (flashed.Count == 100) break;

                foreach (var t in flashed) grid[t.Item1, t.Item2].Flashed = false;
            }

            return step;
        }

        private List<Tuple<int, int>> Process(int i, int j)
        {
            if (i < 0 || j < 0 || i == sizei || j == sizej || grid[i, j].Flashed)
            {
                return new List<Tuple<int, int>>();
            }

            grid[i, j].Level++;
            if (grid[i, j].Level < 10)
            {
                return new List<Tuple<int, int>>();
            }

            grid[i, j].Flashed = true;
            grid[i, j].Level = 0;

            var result = new List<Tuple<int, int>> { new(i, j) };
            result.AddRange(Process(i, j - 1));
            result.AddRange(Process(i - 1, j));
            result.AddRange(Process(i, j + 1));
            result.AddRange(Process(i + 1, j));
            result.AddRange(Process(i - 1, j - 1));
            result.AddRange(Process(i - 1, j + 1));
            result.AddRange(Process(i + 1, j - 1));
            result.AddRange(Process(i + 1, j + 1));

            return result;
        }

        private class Octopus
        {
            public Octopus(int level)
            {
                Level = level;
                Flashed = false;
            }

            public int Level { get; set; }
            public bool Flashed { get; set; }
        }
    }
}