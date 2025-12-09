namespace AdventOfCode.Y2025;

public class Day09() : Solver(2025, 9, "Movie Theater")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        7,1
        11,1
        11,7
        9,7
        9,5
        2,5
        2,3
        7,3
        """, 50, 24),
    ];

    private static (int x, int y)[] ParseVertices(string input) =>
        Parse.Lines(input)
             .Select(line => line.Split(','))
             .Select(p => (x: int.Parse(p[0]), y: int.Parse(p[1])))
             .ToArray();

    public override long Part1(string input)
    {
        var vertices = ParseVertices(input);

        long maxArea = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                long width = Math.Abs(vertices[i].x - vertices[j].x) + 1;
                long height = Math.Abs(vertices[i].y - vertices[j].y) + 1;
                maxArea = Math.Max(maxArea, width * height);
            }
        }

        return maxArea;
    }

    public override long Part2(string input)
    {
        var vertices = ParseVertices(input);

        // Build polygon edges (consecutive vertices connected by horizontal/vertical lines)
        var edges = vertices.Select((v, i) => (v, vertices[(i + 1) % vertices.Length]))
                            .Select(e => (e.v.x, e.v.y, e.Item2.x, e.Item2.y))
                            .ToArray();

        long maxArea = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                var (ax, ay) = vertices[i];
                var (bx, by) = vertices[j];

                int x1 = Math.Min(ax, bx), x2 = Math.Max(ax, bx);
                int y1 = Math.Min(ay, by), y2 = Math.Max(ay, by);

                if (x1 == x2 || y1 == y2) continue;

                // Check no polygon edge crosses rectangle interior
                bool valid = !edges.Any(e => EdgeCrossesRectangle(e, x1, y1, x2, y2));

                if (valid && IsInsidePolygon((x1 + x2) / 2, (y1 + y2) / 2, edges))
                {
                    long area = (long)(x2 - x1 + 1) * (y2 - y1 + 1);
                    maxArea = Math.Max(maxArea, area);
                }
            }
        }

        return maxArea;
    }

    private static bool EdgeCrossesRectangle((int x1, int y1, int x2, int y2) edge, int rx1, int ry1, int rx2, int ry2)
    {
        var (ex1, ey1, ex2, ey2) = edge;

        if (ex1 == ex2) // Vertical edge
        {
            if (ex1 > rx1 && ex1 < rx2) // Edge x strictly inside rectangle
            {
                int minY = Math.Min(ey1, ey2), maxY = Math.Max(ey1, ey2);
                if (minY < ry2 && maxY > ry1) return true;
            }
        }
        else // Horizontal edge
        {
            if (ey1 > ry1 && ey1 < ry2) // Edge y strictly inside rectangle
            {
                int minX = Math.Min(ex1, ex2), maxX = Math.Max(ex1, ex2);
                if (minX < rx2 && maxX > rx1) return true;
            }
        }
        return false;
    }

    private static bool IsInsidePolygon(int px, int py, (int x1, int y1, int x2, int y2)[] edges)
    {
        // Ray casting: count vertical edges to the right
        int crossings = edges.Count(e =>
            e.x1 == e.x2 && e.x1 > px &&
            py >= Math.Min(e.y1, e.y2) && py < Math.Max(e.y1, e.y2));

        return crossings % 2 == 1;
    }
}
