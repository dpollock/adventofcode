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
        var size = 600;
        var grid = BuildGrid(size);

        var ropePositions = BuildRopePositions(grid, size, ropeSize);

        var moves = input.ReadLinesToObject<Move>();

        ProcessMoves(moves, ropePositions, grid);

        var visited = grid.Sum(x => x.Count(t => t.TailVisited > 0));
        return visited;
    }

    public object PartTwo(string input)
    {
        var ropeSize = 10;
        var size = 600;
        var grid = BuildGrid(size);

        var ropePositions = BuildRopePositions(grid, size, ropeSize);

        var moves = input.ReadLinesToObject<Move>();

        ProcessMoves(moves, ropePositions, grid);

        var visited = grid.Sum(x => x.Count(t => t.TailVisited > 0));
        return visited;
    }

    private static void ProcessMoves(IList<Move> moves, LinkedList<Grid> ropePositions, Grid[][] grid)
    {
        foreach (var move in moves)
            for (var i = 0; i < move.Steps; i++)
            {
                var head = ropePositions.First;
                
                head.Value = move.Dir switch
                {
                    "U" => grid[head.Value.Row + 1][head.Value.Col],
                    "D" => grid[head.Value.Row - 1][head.Value.Col],
                    "L" => grid[head.Value.Row][head.Value.Col - 1],
                    "R" => grid[head.Value.Row][head.Value.Col + 1],
                    _ => head.Value
                };

                var processKnot = head;
                while (processKnot?.Next != null)
                {
                    var nextValue = processKnot.Next.Value;
                    var nextValueCol = processKnot.Value.Col - nextValue.Col;
                    var nextValueRow = processKnot.Value.Row - nextValue.Row;

                    if (Math.Abs(nextValueRow) == 2 || Math.Abs(nextValueCol) == 2)
                    {
                        processKnot.Next.Value = grid[nextValue.Row + Math.Sign(nextValueRow)][nextValue.Col + Math.Sign(nextValueCol)];
                    }

                    processKnot = processKnot.Next;
                }

                if (ropePositions.Last != null) ropePositions.Last.Value.TailVisited++;
            }
    }

    private static LinkedList<Grid> BuildRopePositions(Grid[][] grid, int size, int ropeSize)
    {
        var startPosition = grid[size / 2][size / 2];
        var ropePositions = new LinkedList<Grid>();
        for (var i = 0; i < ropeSize; i++) ropePositions.AddFirst(startPosition);

        return ropePositions;
    }

    private static Grid[][] BuildGrid(int size)
    {
        var grid = new Grid[size][];

        for (var i = 0; i < size; i++)
        {
            grid[i] = new Grid[size];
            for (var j = 0; j < size; j++)
                grid[i][j] = new Grid
                {
                    Row = i,
                    Col = j
                };
        }

        return grid;
    }
}

internal record Move(string Dir, int Steps);

internal class Grid
{
    public int Row { get; init; }
    public int Col { get; init; }
    public int TailVisited { get; set; }
}