namespace AdventOfCode.Y2025;

public class Day07() : Solver(2025, 7, "Laboratories")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
       ("""
        .......S.......
        ...............
        .......^.......
        ...............
        ......^.^......
        ...............
        .....^.^.^.....
        ...............
        ....^.^...^....
        ...............
        ...^.^...^.^...
        ...............
        ..^...^.....^..
        ...............
        .^.^.^.^.^...^.
        ...............
        """, 21, 40),
    ];

    public override long Part1(string input) => Simulate(input).splits;

    public override long Part2(string input) => Simulate(input).timelines;

    private static (long splits, long timelines) Simulate(string input)
    {
        var lines = Parse.Lines(input);
        var width = lines[0].Length;
        var startCol = lines[0].IndexOf('S');

        var counts = new long[width];
        counts[startCol] = 1;
        long splits = 0;

        for (var row = 1; row < lines.Length; row++)
        {
            var next = new long[width];
            var line = lines[row];

            for (var col = 0; col < width; col++)
            {
                if (counts[col] == 0) continue;

                if (line[col] == '^')
                {
                    splits++;
                    if (col > 0) next[col - 1] += counts[col];
                    if (col < width - 1) next[col + 1] += counts[col];
                }
                else
                {
                    next[col] += counts[col];
                }
            }
            counts = next;
        }

        return (splits, counts.Sum());
    }
}