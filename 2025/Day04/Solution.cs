namespace AdventOfCode.Y2025;

public class Day04() : Solver(2025, 4, "")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        ..@@.@@@@.
        @@@.@.@.@@
        @@@@@.@.@@
        @.@@@@..@.
        @@.@@@@.@@
        .@@@@@@@.@
        .@.@.@.@@@
        @.@@@.@@@@
        .@@@@@@@@.
        @.@.@@@.@.
        """, 13, -1),
    ];

    public override long Part1(string input)
    {
        var usableSpots = 0;
        var lines = Parse.Grid(input);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                var haveRolls = Grid.Neighbors8((i, j), lines).Where(n => lines[n.row][n.col] == '@').Count();
                if (lines[i][j] == '@' && haveRolls < 4)
                {
                    usableSpots++;
                }
            }
        }
        return usableSpots;
    }

    public override long Part2(string input)
    {
        var lines = Parse.Lines(input);

        return -1;
    }
}