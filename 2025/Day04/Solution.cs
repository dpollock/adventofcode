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

    private static bool IsAccessible(char[][] grid, int row, int col) =>
        grid[row][col] == '@' && (row, col).Neighbors8(grid, c => c == '@').Count() < 4;

    public override long Part1(string input)
    {
        var grid = Parse.Grid(input);
        var count = 0;

        for (int i = 0; i < grid.Length; i++)
            for (int j = 0; j < grid[i].Length; j++)
                if (IsAccessible(grid, i, j))
                    count++;

        return count;
    }

    public override long Part2(string input)
    {
        var grid = Parse.Grid(input);
        var rows = grid.Length;
        var cols = grid[0].Length;
        var totalRemoved = 0;

        // Initial set of candidates - all accessible rolls
        var candidates = new HashSet<(int row, int col)>();
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (IsAccessible(grid, i, j))
                    candidates.Add((i, j));

        while (candidates.Count > 0)
        {
            var toRemove = candidates.ToList();
            candidates.Clear();
            totalRemoved += toRemove.Count;

            // First remove all cells from this wave
            foreach (var (row, col) in toRemove)
                grid[row][col] = '.';

            // Then check neighbors for newly accessible cells
            foreach (var (row, col) in toRemove)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (dr == 0 && dc == 0) continue;
                        var nr = row + dr;
                        var nc = col + dc;
                        if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && IsAccessible(grid, nr, nc))
                            candidates.Add((nr, nc));
                    }
                }
            }
        }

        return totalRemoved;
    }
}