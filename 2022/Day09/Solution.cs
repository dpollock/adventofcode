using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day09;

[ProblemName("Rope Bridge")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var ropeSize = 2;
        var moves = input.ReadLinesToObject<Move>();

        var knots = Enumerable.Range(0, ropeSize).Select(_ => new Point(0, 0)).ToList();
        var uniqueTailPositions = new HashSet<Point> { knots[^1] };

        ProcessMovesV2(moves, knots, uniqueTailPositions);

        var visited = uniqueTailPositions.Count;
        return visited;
    }

    public object PartTwo(string input)
    {
        var ropeSize = 10;
        var moves = input.ReadLinesToObject<Move>();

        var knots = Enumerable.Range(0, ropeSize).Select(_ => new Point(0, 0)).ToList();
        var uniqueTailPositions = new HashSet<Point> { knots[^1] };

        ProcessMovesV2(moves, knots, uniqueTailPositions);

        var visited = uniqueTailPositions.Count;
        return visited;
    }

    private static void ProcessMovesV2(IList<Move> moves, List<Point> knots, HashSet<Point> uniqueTailPositions)
    {
        foreach (var move in moves)
            for (var i = 0; i < move.Steps; i++)
            {
                var headIndex = 0;

                knots[headIndex] = Move(knots[headIndex], move.Direction);

                for (var tailIndex = 1; tailIndex < knots.Count; ++tailIndex)
                {
                    headIndex = tailIndex - 1;
                    var diffX = knots[headIndex].X - knots[tailIndex].X;
                    var diffY = knots[headIndex].Y - knots[tailIndex].Y;

                    if (Math.Abs(diffX) == 2 || Math.Abs(diffY) == 2)
                        knots[tailIndex] = new Point(knots[tailIndex].X + Math.Sign(diffX),
                            knots[tailIndex].Y + Math.Sign(diffY));
                }

                uniqueTailPositions.Add(knots[^1]);
            }
    }


    private static Point Move(Point original, string direction)
    {
        return direction switch
        {
            "U" => original with { Y = original.Y + 1 },
            "D" => original with { Y = original.Y - 1 },
            "L" => original with { X = original.X - 1 },
            "R" => original with { X = original.X + 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unsupported direction")
        };
    }
}

internal record Move(string Direction, int Steps);

internal record Point(int X, int Y);