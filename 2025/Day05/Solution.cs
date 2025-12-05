namespace AdventOfCode.Y2025;

public class Day05() : Solver(2025, 5, "Cafeteria")
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

    private static List<(long start, long end)> ParseRanges(List<string> lines) =>
        lines.Select(line => line.Split('-'))
             .Select(p => (start: long.Parse(p[0]), end: long.Parse(p[1])))
             .ToList();

    public override long Part1(string input)
    {
        var groups = Parse.Groups(input);
        var ranges = ParseRanges(groups[0]);
        var ingredientIds = groups[1].Select(long.Parse);

        return ingredientIds.Count(id => ranges.Any(r => id >= r.start && id <= r.end));
    }

    public override long Part2(string input)
    {
        var groups = Parse.Groups(input);
        var ranges = ParseRanges(groups[0]).OrderBy(r => r.start);

        long count = 0;
        long currentStart = -1, currentEnd = -2;

        foreach (var (start, end) in ranges)
        {
            if (start <= currentEnd + 1)
                currentEnd = Math.Max(currentEnd, end);
            else
            {
                count += currentEnd - currentStart + 1;
                (currentStart, currentEnd) = (start, end);
            }
        }

        return count + currentEnd - currentStart + 1;
    }
}