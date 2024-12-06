using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using SuperLinq;

namespace adventofcode.Lib;

public enum Direction
{
    North,
    South,
    East,
    West,
    NorthWest,
    NorthEast,
    SouthWest,
    SouthEast
}
public static class AoCHelpers
{
    public static readonly IReadOnlyList<(int x, int y, int z)> Neighbors3D =
        new (int x, int y, int z)[] { (0, 1, 0), (0, -1, 0), (1, 0, 0), (-1, 0, 0), (0, 0, -1), (0, 0, 1) };

    public static readonly IReadOnlyList<(int x, int y)> Neighbors =
        new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

    public static readonly IReadOnlyList<(int x, int y)> SurroundingNeighbors =
        new (int x, int y)[] { (0, 1), (0, -1), (1, 0), (-1, 0), (-1, -1), (1, -1), (-1, 1), (1, 1), };


    public static IEnumerable<(int x, int y, int z)> GetCartesianNeighbors(this (int x, int y, int z) p)
    {
        return Neighbors3D.Select(d => (p.x + d.x, p.y + d.y, p.z + d.z));
    }

    public static IEnumerable<(int x, int y)> GetCartesianNeighbors(this (int x, int y) p, bool includeDiagonal = false)
    {
        if (includeDiagonal)
        {
            return SurroundingNeighbors.Select(d => (p.x + d.x, p.y + d.y));
        }
        else
        {
            return Neighbors.Select(d => (p.x + d.x, p.y + d.y));
        }

    }

    public static IEnumerable<(int x, int y)> GetCartesianNeighbors(
        this (int x, int y) p,
        int maxX, int maxY, bool includeDiagonal = false)
    {
        var neighbors = p.GetCartesianNeighbors(includeDiagonal);
        var filtered = neighbors
            .Where(q =>
                q.y >= 0 && q.y < maxY
                         && q.x >= 0 && q.x < maxX);
        return filtered;
    }


    public static IEnumerable<(int x, int y)> GetCartesianNeighbors<T>(
        this (int x, int y) p,
        IReadOnlyList<IReadOnlyList<T>> map, bool includeDiagonal = false)
    {
        return p.GetCartesianNeighbors(includeDiagonal)
            .Where(q =>
                q.y >= 0 && q.y < map.Count
                         && q.x >= 0 && q.x < map[q.y].Count);
    }

    public static (int x, int y)? GetNeighborInDirection<T>(this (int x, int y) p, IReadOnlyList<IReadOnlyList<T>> map, Direction direction)
    {
        var neighbors = p.GetCartesianNeighbors(map.Count, map[0].Count, includeDiagonal: true);
        switch (direction)
        {
            case Direction.North:
                return neighbors.FirstOrDefault(q => q.y == p.y - 1);
            case Direction.South:
                return neighbors.FirstOrDefault(q => q.y == p.y + 1);
            case Direction.East:
                return neighbors.FirstOrDefault(q => q.x == p.x + 1);
            case Direction.West:
                return neighbors.FirstOrDefault(q => q.x == p.x - 1);
            case Direction.NorthWest:
                return neighbors.FirstOrDefault(q => q.x == p.x - 1 && q.y == p.y - 1);
            case Direction.NorthEast:
                return neighbors.FirstOrDefault(q => q.x == p.x + 1 && q.y == p.y - 1);
            case Direction.SouthWest:
                return neighbors.FirstOrDefault(q => q.x == p.x - 1 && q.y == p.y + 1);
            case Direction.SouthEast:
                return neighbors.FirstOrDefault(q => q.x == p.x + 1 && q.y == p.y + 1);
            default:
                throw new ArgumentException($"Unknown direction: {direction}");
        }
    }

    public static char[][] GetCharMap(this string input)
    {
        return input.Segment(b => b == '\n')
            .Select(l => l
                .SkipWhile(b => b == '\n')
                .Select(b => b)
                .ToArray())
            .Where(l => l.Length > 0)
            .ToArray();
    }

    public static char[][] ToCharGrid(this string input)
    {
        var lines = input.ReadLinesToType<string>().ToArray();
        var platform = new char[lines.Length][];
        for (var y = 0; y < lines.Length; y++)
            platform[y] = lines[y].ToCharArray();
        return platform;
    }

    public static IEnumerable<T> ReadLinesToType<T>(this string s) where T : IConvertible
    {
        using var sr = new StringReader(s);
        while (sr.ReadLine() is { } line)
            yield return (T)Convert.ChangeType(line, typeof(T));
    }

    public static IEnumerable<List<T>> ReadLinesToGroupsOfType<T>(this string s) where T : IConvertible
    {
        var result = new List<List<T>>();
        var currentGroup = new List<T>();
        using var sr = new StringReader(s);
        while (sr.ReadLine() is { } line)
            if (string.IsNullOrEmpty(line))
            {
                result.Add(currentGroup);
                currentGroup = new List<T>();
            }
            else
            {
                currentGroup.Add((T)Convert.ChangeType(line, typeof(T)));
            }

        if (currentGroup.Any())
            result.Add(currentGroup);

        return result;
    }

    public static IList<T> ReadLinesToObject<T>(this string s, string fieldDelimiter = " ")
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = fieldDelimiter,
            WhiteSpaceChars = Array.Empty<char>(),
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.None,
            MissingFieldFound = args => { }
        };

        using var reader = new StringReader(s);
        using var csv = new CsvReader(reader, config);
        return csv.GetRecords<T>().ToList();
    }


    public static IEnumerable<T> ReadLines<T>(this string s, Func<string, T> converter)
    {
        using var sr = new StringReader(s);
        while (sr.ReadLine() is { } line)
            yield return converter(line);
    }
}