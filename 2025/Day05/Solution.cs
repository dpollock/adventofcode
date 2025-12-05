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
        """, 3, null),
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
        var lines = Parse.Lines(input);

        return -1;
    }
}