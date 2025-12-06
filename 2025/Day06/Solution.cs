namespace AdventOfCode.Y2025;

public class Day06() : Solver(2025, 6, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  ", 4277556, 3263827),
    ];

    public override long Part1(string input) => SolvePart1(input);
    public override long Part2(string input) => SolvePart2(input);

    private static long SolvePart1(string input)
    {
        var lines = Parse.Lines(input);
        var ops = lines[^1];
        int numRows = lines.Length - 1;

        // Parse all numbers and operations upfront
        var grid = new long[numRows][];
        for (int r = 0; r < numRows; r++)
            grid[r] = lines[r].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

        var operations = ops.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int numCols = grid[0].Length;

        long sum = 0;
        for (int col = 0; col < numCols; col++)
        {
            bool multiply = operations[col][0] == '*';
            long result = grid[0][col];
            for (int row = 1; row < numRows; row++)
                result = multiply ? result * grid[row][col] : result + grid[row][col];
            sum += result;
        }
        return sum;
    }

    private static long SolvePart2(string input)
    {
        var lines = Parse.Lines(input);
        var opLine = lines[^1];
        int numRows = lines.Length - 1;

        // Find problem boundaries by locating operators (left-aligned)
        var problems = new List<(int start, int end, bool multiply)>();
        int probStart = -1;
        for (int i = 0; i < opLine.Length; i++)
        {
            char c = opLine[i];
            if (c == '*' || c == '+')
            {
                if (probStart >= 0)
                    problems.Add((probStart, i, opLine[probStart] == '*'));
                probStart = i;
            }
        }
        if (probStart >= 0)
            problems.Add((probStart, opLine.Length, opLine[probStart] == '*'));

        long sum = 0;
        foreach (var (start, end, multiply) in problems)
        {
            // Each column position = one number (digits stacked top-to-bottom)
            // Process columns right-to-left
            long result = 0;
            bool first = true;

            for (int pos = end - 1; pos >= start; pos--)
            {
                long num = 0;
                bool hasDigit = false;
                for (int row = 0; row < numRows; row++)
                {
                    var line = lines[row];
                    if (pos < line.Length && char.IsAsciiDigit(line[pos]))
                    {
                        num = num * 10 + (line[pos] - '0');
                        hasDigit = true;
                    }
                }

                if (hasDigit)
                {
                    if (first) { result = num; first = false; }
                    else result = multiply ? result * num : result + num;
                }
            }
            sum += result;
        }
        return sum;
    }
}