using System;
using System.Linq;
using adventofcode.Lib;
using SuperLinq;

namespace AdventOfCode.Y2022.Day12;

[ProblemName("Hill Climbing Algorithm")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var values = BuildMap(input);

        // Feels like cheating using SuperLinq to use A* path finding
        var cost = SuperEnumerable.GetShortestPathCost<(int x, int y), int>(
            values.start,
            (p, c) => p.GetCartesianNeighbors(values.map)
                .Where(q => values.map[q.y][q.x] - values.map[p.y][p.x] <=
                            1) // cost if going up hill is at most 1, but if going down hill can be any value
                .Select(q => (q, c + 1)),
            values.end);

        return cost.ToString();
    }


    public object PartTwo(string input)
    {
        var values = BuildMap(input);

        var minCost = int.MaxValue;
        for (var y = 0; y < values.map.Length; y++)
        for (var x = 0; x < values.map[y].Length; x++)
            if (values.map[y][x] == 'a')
                try
                {
                    var curCost = SuperEnumerable.GetShortestPathCost<(int x, int y), int>(
                        (x, y),
                        (p, c) => p.GetCartesianNeighbors(values.map)
                            .Where(q => values.map[q.y][q.x] - values.map[p.y][p.x] <= 1)
                            .Select(q => (q, c + 1)),
                        values.end);

                    minCost = Math.Min(minCost, curCost);
                }
                catch
                {
                    // GetShortestPathCost throws an exception if there's not a path to be found, just ignore those.

}

        return minCost;
    }

    private static (char[][] map, (int x, int y) start, (int x, int y) end) BuildMap(string input)
    {
        var start = (x: 0, y: 0);
        var end = start;
        var map = input.GetCharMap();

        for (var y = 0; y < map.Length; y++)
        for (var x = 0; x < map[y].Length; x++)
            if (map[y][x] == 'S')
                start = (x, y);
            else if (map[y][x] == 'E') end = (x, y);

        map[start.y][start.x] = 'a';
        map[end.y][end.x] = 'z';

        return (map, start, end);
    }
}