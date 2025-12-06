namespace AdventOfCode.Y2025;

public class Day06() : Solver(2025, 6, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        123 328  51 64 
        45 64  387 23 
        6 98  215 314
        *   +   *   +  
        """, 4277556, -1),
    ];

    public override long Part1(string input)
    {
        var lines = Parse.Lines(input);

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
}