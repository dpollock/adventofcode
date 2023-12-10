using System;
using System.Collections.Generic;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var values = BuildMap(input);

        var i = 1;
        ((int x, int y), Direction)? result = ((values.start.x + 1, values.start.y), Direction.West);
        do
        {
            i++;
            result = Travel2(values.map, result.Value.Item1, result.Value.Item2);
        } while (result != null && result.Value.Item1 != values.start && i > 1);

        return i / 2;
    }

    public object PartTwo(string input)
    {
        var values = BuildMap(input);

        var i = 1;

        ((int x, int y), Direction)? result = ((values.start.x + 1, values.start.y), Direction.West);
        do
        {
            i++;
            result = Travel2(values.map, result.Value.Item1, result.Value.Item2);
        } while (result != null && result.Value.Item1 != values.start && i > 1);

        return i / 2;
    }

    private ((int x, int y), Direction)? Travel2(char[][] map, (int x, int y) current, Direction enterDirection)
    {
        var result = new List<(int x, int y)> { current };

        Console.WriteLine($"Current: {current.x}, {current.y}, {map[current.y][current.x]}");
        if (map[current.y][current.x] == '|')
            switch (enterDirection)
            {
                case Direction.North:
                {
                    var south = current.GetNeighborInDirection(Direction.South, map);
                    if (south != null)
                        return new ValueTuple<(int x, int y), Direction>(south.Value, Direction.North);
                    break;
                }
                case Direction.South:
                {
                    var north = current.GetNeighborInDirection(Direction.North, map);
                    if (north != null)
                        return new ValueTuple<(int x, int y), Direction>(north.Value, Direction.South);
                    break;
                }
            }

        if (map[current.y][current.x] == '-')
            switch (enterDirection)
            {
                case Direction.West:
                {
                    var south = current.GetNeighborInDirection(Direction.East, map);
                    if (south != null)
                        return new ValueTuple<(int x, int y), Direction>(south.Value, Direction.West);
                    break;
                }
                case Direction.East:
                {
                    var north = current.GetNeighborInDirection(Direction.West, map);
                    if (north != null)
                        return new ValueTuple<(int x, int y), Direction>(north.Value, Direction.East);
                    break;
                }
            }

        if (map[current.y][current.x] == 'L')
            switch (enterDirection)
            {
                case Direction.North:
                {
                    var east = current.GetNeighborInDirection(Direction.East, map);
                    if (east != null)
                        return new ValueTuple<(int x, int y), Direction>(east.Value, Direction.West);
                    break;
                }
                case Direction.East:
                {
                    var north = current.GetNeighborInDirection(Direction.North, map);
                    if (north != null)
                        return new ValueTuple<(int x, int y), Direction>(north.Value, Direction.South);
                    break;
                }
            }

        if (map[current.y][current.x] == 'J')
            switch (enterDirection)
            {
                case Direction.North:
                {
                    var west = current.GetNeighborInDirection(Direction.West, map);
                    if (west != null)
                        return new ValueTuple<(int x, int y), Direction>(west.Value, Direction.East);
                    break;
                }
                case Direction.West:
                {
                    var north = current.GetNeighborInDirection(Direction.North, map);
                    if (north != null)
                        return new ValueTuple<(int x, int y), Direction>(north.Value, Direction.South);
                    break;
                }
            }

        if (map[current.y][current.x] == '7')
            switch (enterDirection)
            {
                case Direction.South:
                {
                    var west = current.GetNeighborInDirection(Direction.West, map);
                    if (west != null)
                        return new ValueTuple<(int x, int y), Direction>(west.Value, Direction.East);
                    break;
                }
                case Direction.West:
                {
                    var south = current.GetNeighborInDirection(Direction.South, map);
                    if (south != null)
                        return new ValueTuple<(int x, int y), Direction>(south.Value, Direction.North);
                    break;
                }
            }

        if (map[current.y][current.x] == 'F')
            switch (enterDirection)
            {
                case Direction.South:
                {
                    var east = current.GetNeighborInDirection(Direction.East, map);
                    if (east != null)
                        return new ValueTuple<(int x, int y), Direction>(east.Value, Direction.West);
                    break;
                }
                case Direction.East:
                {
                    var south = current.GetNeighborInDirection(Direction.South, map);
                    if (south != null)
                        return new ValueTuple<(int x, int y), Direction>(south.Value, Direction.North);
                    break;
                }
            }


        return null;
    }

    private static (char[][] map, (int x, int y) start) BuildMap(string input)
    {
        var start = (x: 0, y: 0);
        var map = input.GetCharMap();

        for (var y = 0; y < map.Length; y++)
        for (var x = 0; x < map[y].Length; x++)
            if (map[y][x] == 'S')
                start = (x, y);

        return (map, start);
    }
}