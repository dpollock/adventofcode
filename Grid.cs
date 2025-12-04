namespace AdventOfCode;

public enum Dir { N, S, E, W, NE, NW, SE, SW }

public static class Grid
{
    private static readonly (int dr, int dc)[] Offsets4 = [(-1, 0), (1, 0), (0, -1), (0, 1)];
    private static readonly (int dr, int dc)[] Offsets8 = [(-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1)];

    public static IEnumerable<(int row, int col)> Neighbors4(this (int row, int col) p) =>
        Offsets4.Select(o => (p.row + o.dr, p.col + o.dc));

    public static IEnumerable<(int row, int col)> Neighbors8(this (int row, int col) p) =>
        Offsets8.Select(o => (p.row + o.dr, p.col + o.dc));

    public static IEnumerable<(int row, int col)> Neighbors4<T>(this (int row, int col) p, T[][] grid, Func<T, bool>? predicate = null) =>
        p.Neighbors4().Where(n => n.InBounds(grid) && (predicate == null || predicate(grid[n.row][n.col])));

    public static IEnumerable<(int row, int col)> Neighbors8<T>(this (int row, int col) p, T[][] grid, Func<T, bool>? predicate = null) =>
        p.Neighbors8().Where(n => n.InBounds(grid) && (predicate == null || predicate(grid[n.row][n.col])));

    public static bool InBounds<T>(this (int row, int col) p, T[][] grid) =>
        p.row >= 0 && p.row < grid.Length && p.col >= 0 && p.col < grid[0].Length;

    public static (int row, int col) Move(this (int row, int col) p, Dir dir) => dir switch
    {
        Dir.N => (p.row - 1, p.col),
        Dir.S => (p.row + 1, p.col),
        Dir.W => (p.row, p.col - 1),
        Dir.E => (p.row, p.col + 1),
        Dir.NW => (p.row - 1, p.col - 1),
        Dir.NE => (p.row - 1, p.col + 1),
        Dir.SW => (p.row + 1, p.col - 1),
        Dir.SE => (p.row + 1, p.col + 1),
        _ => p
    };
}
