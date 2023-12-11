using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day11;

[ProblemName("Cosmic Expansion")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var sum = Solve(input, 2);
        return sum;
    }

    public object PartTwo(string input)
    {
        var sum = Solve(input, 1000000);
        return sum;
    }

    private static long Solve(string input, int expansion)
    {
        var (size, emptyRows, emptyCols, galaxies) = GetGalaxies(input);

        var rowMapping = Enumerable.Range(0, size).Select(r => r + (expansion - 1) * emptyRows.Count(e => e < r))
            .ToArray();
        var colMapping = Enumerable.Range(0, size).Select(c => c + (expansion - 1) * emptyCols.Count(e => e < c))
            .ToArray();

        long sum = 0;
        for (var i = 0; i < galaxies.Count; i++)
        {
            var iRow = rowMapping[galaxies[i].x];
            var iCol = colMapping[galaxies[i].y];
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                var jRow = rowMapping[galaxies[j].x];
                var jCol = colMapping[galaxies[j].y];
                sum += Math.Abs(jRow - iRow) + Math.Abs(jCol - iCol);
            }
        }

        return sum;
    }

    private static (int size, HashSet<int> emptyRows, HashSet<int> emptyCols, List<(long x, long y)> galaxies)
        GetGalaxies(string input)
    {
        var lines = input.ReadLinesToType<string>().ToList();
        var size = lines.Count;
        var emptyRows = new HashSet<int>(Enumerable.Range(0, size));
        var emptyCols = new HashSet<int>(Enumerable.Range(0, size));

        var galaxies = new List<(long x, long y)>();
        for (var i = 0; i < size; i++)
        for (var j = 0; j < size; j++)
            if (lines[i][j] == '#')
            {
                emptyRows.Remove(i);
                emptyCols.Remove(j);
                galaxies.Add((i, j));
            }

        return (size, emptyRows, emptyCols, galaxies);
    }
}