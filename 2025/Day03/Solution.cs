namespace AdventOfCode.Y2025;

public class Day03() : Solver(2025, 3, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
          ("""
        987654321111111
        811111111111119
        234234234234278
        818181911112111
        """, 357, 3121910778619)
    ];

    public override long Part1(string input)
    {
        return FindSumOfMaxJoltages(input, 2);
    }

    public override long Part2(string input)
    {
        return FindSumOfMaxJoltages(input, 12);
    }

    private static long FindSumOfMaxJoltages(string input, int numberOfDigits)
        => Parse.IntGrid(input).Sum(line => FindLargestNumber(line, numberOfDigits));

    private static long FindLargestNumber(int[] line, int count)
    {
        long result = 0;
        int start = 0;

        for (int i = 0; i < count; i++)
        {
            var segment = line.AsSpan(start, line.Length - count + i - start + 1);
            int maxIdx = start + segment.IndexOf(segment.ToArray().Max());

            result = result * 10 + line[maxIdx];
            start = maxIdx + 1;
        }

        return result;
    }
}