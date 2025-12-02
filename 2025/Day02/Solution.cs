namespace AdventOfCode.Y2025;

public class Day02() : Solver(2025, 2, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124""", 1227775554, 0),
    ];

    public override long Part1(string input)
    {
        var ranges = Parse.Regex(input, @"(\d+)-(\d+)", m => (long.Parse(m.Groups[1].Value), long.Parse(m.Groups[2].Value)), ",");

        long sum = 0;
        foreach (var (start, end) in ranges)
        {
            sum += SumInvalidIdsInRange(start, end);
        }
        return sum;
    }

    private static long SumInvalidIdsInRange(long start, long end)
    {
        long sum = 0;
        // Generate invalid IDs and check if they fall in range
        // Invalid IDs are formed by repeating a pattern: 1-9, 10-99, 100-999, etc.
        // Max pattern we need is based on end value - a 20-digit number needs 10-digit pattern
        int maxDigits = end.ToString().Length / 2 + 1;

        for (int digits = 1; digits <= maxDigits && digits <= 9; digits++)
        {
            long minPattern = digits == 1 ? 1 : (long)Math.Pow(10, digits - 1);
            long maxPattern = (long)Math.Pow(10, digits) - 1;

            for (long pattern = minPattern; pattern <= maxPattern; pattern++)
            {
                // Create the invalid ID by repeating the pattern twice
                long multiplier = (long)Math.Pow(10, digits);
                long invalidId = pattern * multiplier + pattern;

                if (invalidId > end) break;

                if (invalidId >= start)
                {
                    sum += invalidId;
                }
            }
        }
        return sum;
    }

    public override long Part2(string input)
    {
        return 0; // Part 2 not yet available
    }
}