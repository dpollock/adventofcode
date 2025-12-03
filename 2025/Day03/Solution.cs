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
        var lines = Parse.Lines(input, line => line.Select(c => int.Parse(c.ToString())).ToArray());
        var sumOfMaxJoltages = 0;
        foreach (var line in lines)
        {
            var firstMax = line[..^1].Max(); //look at all but the last digit
            var firstMaxIndex = Array.IndexOf(line, firstMax);
            var secondMax = line[(firstMaxIndex + 1)..].Max();

            sumOfMaxJoltages += firstMax * 10 + secondMax;
        }
        return sumOfMaxJoltages;
    }

    public override long Part2(string input)
    {
        var lines = Parse.Lines(input, line => line.Select(c => int.Parse(c.ToString())).ToArray());
        var sumOfMaxJoltages = 0L;
        foreach (var line in lines)
        {
            //find the largest number formed by 12 digits (not necessarily consecutive)
            var largestNumber = FindLargestNumberRecursively(line, 12);
            sumOfMaxJoltages += largestNumber;
        }
        return sumOfMaxJoltages; //return the sum of the largest numbers
    }

    private static long FindLargestNumberRecursively(int[] line, int maxDigitsLength)
    {
        if (maxDigitsLength > line.Length || maxDigitsLength == 0)
        {
            return 0L;
        }
        else
        {
            var firstMax = line[..^(maxDigitsLength - 1)].Max();
            var firstMaxIndex = Array.IndexOf(line, firstMax);
            return (firstMax * (long)Math.Pow(10, maxDigitsLength - 1)) + FindLargestNumberRecursively(line[(firstMaxIndex + 1)..], maxDigitsLength - 1);
        }
    }
}