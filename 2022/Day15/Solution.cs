using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day15;

[ProblemName("Beacon Exclusion Zone")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var checkY = 2000000;
        var (beacons, sensors) = ParseInput(input);

        sensors.Sort((left, right) => left.Position.X.CompareTo(right.Position.X));
        beacons.Sort((left, right) => left.Position.X.CompareTo(right.Position.X));

        var leftMostSensorRadiusEdge =
            sensors.Select(sensor => sensor.Position.X - sensor.DistanceToBeacon).MinBy(x => x);
        var rightMostSensorRadiusEdge = sensors.Select(sensor => sensor.Position.X + sensor.DistanceToBeacon)
            .MaxBy(x => x);

        var rangeStart = Math.Min(leftMostSensorRadiusEdge, beacons[0].Position.X);
        var rangeEnd = Math.Max(rightMostSensorRadiusEdge, beacons[^1].Position.X);

        long invalidPositionCount = 0;

        for (var x = rangeStart; x < rangeEnd; ++x)
        {
            var position = new Coordinate(x, checkY);
            if (sensors.Any(sensor =>
                    position != sensor.ClosestBeacon.Position &&
                    sensor.DistanceToBeacon >= sensor.DistanceTo(position))) ++invalidPositionCount;
        }

        return invalidPositionCount;
    }

    public object PartTwo(string input)
    {
        var upperLimit = 4000000;
        const int tuningFrequencyMultiplier = 4000000;

        var (_, sensors) = ParseInput(input);
        var edgeCoordinates = sensors
            .Select(GetEdgeCoordinates)
            .SelectMany(coordinate => coordinate);

        foreach (var coordinate in edgeCoordinates)
            if (coordinate.X > 0 && coordinate.X < upperLimit &&
                coordinate.Y > 0 && coordinate.Y < upperLimit)
            {
                var inSensorRange = sensors.Any(sensor => sensor.DistanceToBeacon >= sensor.DistanceTo(coordinate));

                if (!inSensorRange) return coordinate.X * tuningFrequencyMultiplier + coordinate.Y;
            }

        return -1;
    }

    private static (List<Beacon> beacons, List<Sensor> sensors) ParseInput(string input)
    {
        var regex = new Regex(@"(-?[0-9])\w+");
        var lines = input.ReadLinesToType<string>();
        var beacons = new List<Beacon>();
        var sensors = new List<Sensor>();

        foreach (var line in lines)
        {
            var matches = regex.Matches(line);

            var beacon = new Beacon
                { Position = new Coordinate(long.Parse(matches[2].Value), long.Parse(matches[3].Value)) };
            var sensor = new Sensor
            {
                ClosestBeacon = beacon,
                Position = new Coordinate(long.Parse(matches[0].Value), long.Parse(matches[1].Value))
            };
            beacons.Add(beacon);
            sensors.Add(sensor);
        }

        return (beacons, sensors);
    }

    private static List<Coordinate> GetEdgeCoordinates(Sensor sensor)
    {
        var left = sensor.Position with { X = sensor.Position.X - sensor.DistanceToBeacon - 1 };
        var right = sensor.Position with { X = sensor.Position.X + sensor.DistanceToBeacon + 1 };
        var top = sensor.Position with { Y = sensor.Position.Y + sensor.DistanceToBeacon + 1 };
        var bottom = sensor.Position with { Y = sensor.Position.Y - sensor.DistanceToBeacon - 1 };

        var edges = new List<Coordinate>();

        edges = edges
            .Concat(GetCoordinatesBetween(left, top))
            .Concat(GetCoordinatesBetween(bottom, right))
            .Concat(GetCoordinatesBetween(top, right))
            .Concat(GetCoordinatesBetween(left, bottom))
            .ToList();

        return edges;
    }

    private static IEnumerable<Coordinate> GetCoordinatesBetween(Coordinate start, Coordinate end)
    {
        var (left, right) = start.X <= end.X ? (start, end) : (end, start);
        var coordinates = new List<Coordinate>();

        if (left.Y <= end.Y)
            for (var offset = 0; offset <= right.X - left.X; ++offset)
                coordinates.Add(new Coordinate(left.X + offset, left.Y + offset));
        else
            for (var offset = 0; offset <= right.X - left.X; ++offset)
                coordinates.Add(new Coordinate(left.X + offset, left.Y - offset));

        return coordinates;
    }
}

internal class Beacon
{
    public Coordinate Position { get; set; } = new(0, 0);
}

internal record Coordinate(long X, long Y);

internal class Sensor
{
    public Coordinate Position { get; set; } = new(0, 0);
    public Beacon ClosestBeacon { get; set; } = new();

    public long DistanceToBeacon => DistanceTo(ClosestBeacon.Position);

    public long DistanceTo(Coordinate position)
    {
        return Math.Abs(Position.X - position.X) +
               Math.Abs(Position.Y - position.Y);
    }
}