namespace AdventOfCode.Y2025;

public class Day06() : Solver(2025, 6, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  ", 4277556, 3263827),
    ];

    public override long Part1(string input) => Solve(input, pad: false);

    public override long Part2(string input) => SolvePart2(input);

    private static long Solve(string input, bool pad)
    {
        var lines = Parse.Lines(input);

        // Part 1: just split on whitespace
        var grid = lines[..^1].Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray()).ToArray();
        var operations = lines[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToList();

        long sum = 0;
        for (int col = 0; col < grid[0].Length; col++)
        {
            var result = grid.Select(row => row[col]).Aggregate((a, b) => operations[col] == '+' ? a + b : a * b);
            sum += result;
        }
        return sum;
    }

    private static long SolvePart2(string input)
    {
        var lines = Parse.Lines(input);
        var dataLines = lines[..^1];
        var opLine = lines[^1];

        // Find column starts by locating operators (left-aligned)
        var colStarts = new List<int>();
        for (int i = 0; i < opLine.Length; i++)
            if (opLine[i] == '*' || opLine[i] == '+')
                colStarts.Add(i);
        colStarts.Add(opLine.Length);

        long sum = 0;
        for (int prob = 0; prob < colStarts.Count - 1; prob++)
        {
            int start = colStarts[prob];
            int end = colStarts[prob + 1];
            char op = opLine[start];

            // Each CHARACTER POSITION within the problem is a separate number
            // Digits at that position across all rows form the number (top=MSB, bottom=LSB)
            // Process positions right-to-left
            var numbers = new List<long>();
            for (int pos = end - 1; pos >= start; pos--)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var line in dataLines)
                {
                    char c = pos < line.Length ? line[pos] : ' ';
                    if (char.IsDigit(c))
                        sb.Append(c);
                }
                if (sb.Length > 0)
                    numbers.Add(long.Parse(sb.ToString()));
            }

            if (numbers.Count == 0) continue;

            long result = numbers[0];
            for (int i = 1; i < numbers.Count; i++)
                result = op == '+' ? result + numbers[i] : result * numbers[i];

            sum += result;
        }
        return sum;
    }
}