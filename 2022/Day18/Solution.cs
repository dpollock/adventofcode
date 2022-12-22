using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day18;

[ProblemName("Boiling Boulders")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var cubes = input.ReadLinesToType<string>().Select(l =>
        {
            var split = l.Split(',');
            return new Cube { Point = (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])) };
        }).ToList();

        foreach (var cube in cubes)
        {
            var neighbors = cube.Point.GetCartesianNeighbors();
            foreach (var neighbor in neighbors)
                if (cubes.FirstOrDefault(c => c.Point == neighbor) != null)
                    cube.ExposedFaces--;
        }

        var result = cubes.Sum(c => c.ExposedFaces);
        return result;
    }

    public object PartTwo(string input)
    {
        var cubes = input.ReadLinesToType<string>().Select(l =>
        {
            var split = l.Split(',');
            return new Cube { Point = (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])) };
        }).ToList();

        var (xmin, xmax) = (cubes.Min(c => c.Point.x), cubes.Max(c => c.Point.x));
        var (ymin, ymax) = (cubes.Min(c => c.Point.y), cubes.Max(c => c.Point.y));
        var (zmin, zmax) = (cubes.Min(c => c.Point.z), cubes.Max(c => c.Point.z));

       

        var filled = new List<(int x, int y, int z)>();

        var result = 0;


        void FloodFill((int x, int y, int z) point)
        {
            if (filled.Any(c => c == point)) return;
            if (cubes.Any(c => c.Point == point)) return;
            if (point.x < xmin || point.x > xmax ||
                point.y < ymin || point.y > ymax ||
                point.z < zmin || point.z > zmax)
                return;


            filled.Add(point);

            var neighbors = point.GetCartesianNeighbors();
            foreach (var neighbor in neighbors) FloodFill(neighbor);
        }

        FloodFill((xmin, ymin, zmin));

        foreach (var cube in filled)
        {
            //result += 6;
            var neighbors = cube.GetCartesianNeighbors();
            result += neighbors.Count(neighbor => cubes.Any(c => c.Point == neighbor));
        }


        //expect 58
        return result;
    }
}

internal class Cube
{
    public (int x, int y, int z) Point;

    public int ExposedFaces { get; set; } = 6;
}