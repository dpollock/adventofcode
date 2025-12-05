namespace AdventOfCode.Y2025;

public class Day05() : Solver(2025, 5, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        3-5
        10-14
        16-20
        12-18

        1
        5
        8
        11
        17
        32
        """, 3, 14),
    ];

    public override long Part1(string input)
    {
        var groups = Parse.Groups(input);

        // Parse fresh ranges
        var ranges = groups[0]
            .Select(line =>
            {
                var parts = line.Split('-');
                return (start: long.Parse(parts[0]), end: long.Parse(parts[1]));
            })
            .ToList();

        // Parse available ingredient IDs
        var ingredientIds = groups[1].Select(long.Parse).ToList();

        // Count fresh ingredients (those in any range)
        return ingredientIds.Count(id => ranges.Any(r => id >= r.start && id <= r.end));
    }

    public override long Part2(string input)
    {
        var groups = Parse.Groups(input);

        // Parse fresh ranges
        var ranges = groups[0]
            .Select(line =>
            {
                var parts = line.Split('-');
                return (start: long.Parse(parts[0]), end: long.Parse(parts[1]));
            })
            .OrderBy(r => r.start)
            .ToList();

        // Merge overlapping ranges and count total fresh IDs
        long count = 0;
        var (currentStart, currentEnd) = ranges[0];

        for (int i = 1; i < ranges.Count; i++)
        {
            var (start, end) = ranges[i];
            if (start <= currentEnd + 1)
            {
                // Overlapping or adjacent - extend current range
                currentEnd = Math.Max(currentEnd, end);
            }
            else
            {
                // No overlap - add current range count and start new range
                count += currentEnd - currentStart + 1;
                currentStart = start;
                currentEnd = end;
            }
        }

        // Add the last range
        count += currentEnd - currentStart + 1;

        return count;
    }
}