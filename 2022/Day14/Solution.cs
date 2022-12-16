using System;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day14;

[ProblemName("Regolith Reservoir")]
internal class Solution : ISolver
{
    //input maxes: 
    // X-> (465, 527)
    // Y-> (14, 166)
    private const int maxX = 527 + 200;
    private const int maxY = 166 + 3;
    private const int xOffSet = 0;
    private static readonly Point Start = new(500 - xOffSet, 0);


    public object PartOne(string input)
    {
        var cave = BuildCave(input, false);
        var numOfSands = RunSand(cave);
        //PrintCave(cave);
        return numOfSands;
    }

    public object PartTwo(string input)
    {
        var cave = BuildCave(input, true);
        var numOfSands = RunSand(cave);
        PrintCave(cave);
        return numOfSands;
    }

    private static int RunSand(char[,] cave)
    {
        var numOfSands = 0;
        var intoTheAbyss = false;
        while (!intoTheAbyss)
        {
            var sand = Start with { };
            var sandMoving = true;
            while (sandMoving)
            {

               

                if (sand.Y >= maxY - 1)
                {
                    sandMoving = false;
                    intoTheAbyss = true;
                    continue;
                }

                if (cave[sand.X, sand.Y + 1] == char.MinValue)
                {
                    sand = new Point(sand.X, sand.Y + 1);
                    continue;
                }

                if (cave[sand.X - 1, sand.Y + 1] == char.MinValue)
                {
                    sand = new Point(sand.X - 1, sand.Y + 1);
                    continue;
                }

                if (cave[sand.X + 1, sand.Y + 1] == char.MinValue)
                {
                    sand = new Point(sand.X + 1, sand.Y + 1);
                    continue;
                }

                numOfSands++;
                cave[sand.X, sand.Y] = 'o';
                sandMoving = false;
                if (sand.Y == 0) intoTheAbyss = true;
            }
        }

        return numOfSands;
    }

    private static void PrintCave(char[,] cave)
    {
        var startSand = new Point(500 - xOffSet, 0);
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX - xOffSet; x++)
                if (startSand == new Point(x, y))
                    Console.Write("S");
                else
                    Console.Write(cave[x, y] == char.MinValue ? "." : cave[x, y]);

            Console.WriteLine();
        }
    }

    private static char[,] BuildCave(string input, bool hasFloor)
    {
        var cave = new char[maxX - xOffSet, maxY];
        var lines = input.ReadLinesToType<string>();
        foreach (var line in lines)
        {
            var walls = line.Replace(" -> ", " ").Split(' ')
                .Select(x => new Point(int.Parse(x.Split(',')[0]) - xOffSet, int.Parse(x.Split(',')[1])));

            var start = walls.First();
            foreach (var end in walls.Skip(1))
            {
                if (start.X == end.X)
                {
                    var startY = start.Y;
                    var endY = end.Y;
                    if (start.Y > end.Y)
                    {
                        startY = end.Y;
                        endY = start.Y;
                    }

                    for (var y = startY; y <= endY; y++)
                        cave[start.X, y] = '#';
                }
                else
                {
                    var startX = start.X;
                    var endX = end.X;
                    if (start.X > end.X)
                    {
                        startX = end.X;
                        endX = start.X;
                    }

                    for (var x = startX; x <= endX; x++)
                        cave[x, start.Y] = '#';
                }

                start = end;
            }
        }

        if (hasFloor)
            for (var x = 0; x < maxX - xOffSet; x++)
                cave[x, maxY - 1] = '#';

        return cave;
    }
}

internal record Point(int X, int Y);