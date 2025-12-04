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
        """, 13, 43),
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
        var grid = Parse.Grid(input);
        var totalRemoved = 0;

        while (true)
        {
            var toRemove = new List<(int row, int col)>();

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == '@')
                    {
                        var neighborRolls = (i, j).Neighbors8(grid).Count(n => grid[n.row][n.col] == '@');
                        if (neighborRolls < 4)
                        {
                            toRemove.Add((i, j));
                        }
                    }
                }
            }

            if (toRemove.Count == 0)
                break;

            foreach (var (row, col) in toRemove)
            {
                grid[row][col] = '.';
            }

            totalRemoved += toRemove.Count;
        }

        return totalRemoved;
    }
}