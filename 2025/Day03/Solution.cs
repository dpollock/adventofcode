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
    {
        var lines = Parse.IntGrid(input);
        var sumOfMaxJoltages = 0L;
        foreach (var line in lines)
        {
            var largestNumber = FindLargestNumberRecursively(line, numberOfDigits);
            sumOfMaxJoltages += largestNumber;
        }
        return sumOfMaxJoltages;
    }

    private static long FindLargestNumberRecursively(int[] line, int maxDigitsLength)
    {
        if (maxDigitsLength > line.Length || maxDigitsLength == 0)
        {
            return 0L;
        }

        var firstMax = line[..^(maxDigitsLength - 1)].Max();
        var firstMaxIndex = Array.IndexOf(line, firstMax);
        return (firstMax * (long)Math.Pow(10, maxDigitsLength - 1)) + FindLargestNumberRecursively(line[(firstMaxIndex + 1)..], maxDigitsLength - 1);
    }
}