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
        int n = vertices.Length;

        // Pre-sort edges into sorted arrays for binary search
        var verticalEdges = new List<(int x, int minY, int maxY)>();
        var horizontalEdges = new List<(int y, int minX, int maxX)>();

        for (int i = 0; i < n; i++)
        {
            var (x1, y1) = vertices[i];
            var (x2, y2) = vertices[(i + 1) % n];

            if (x1 == x2)
                verticalEdges.Add((x1, Math.Min(y1, y2), Math.Max(y1, y2)));
            else
                horizontalEdges.Add((y1, Math.Min(x1, x2), Math.Max(x1, x2)));
        }

        // Sort for efficient range queries
        var vEdgesByX = verticalEdges.OrderBy(e => e.x).ToArray();
        var hEdgesByY = horizontalEdges.OrderBy(e => e.y).ToArray();
        var vEdgeXs = vEdgesByX.Select(e => e.x).ToArray();
        var hEdgeYs = hEdgesByY.Select(e => e.y).ToArray();

        long maxArea = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var (ax, ay) = vertices[i];
                var (bx, by) = vertices[j];

                int x1 = Math.Min(ax, bx), x2 = Math.Max(ax, bx);
                int y1 = Math.Min(ay, by), y2 = Math.Max(ay, by);

                if (x1 == x2 || y1 == y2) continue;

                // Check vertical edges crossing rectangle (x in (x1,x2), y overlaps [y1,y2])
                int vStart = LowerBound(vEdgeXs, x1 + 1);
                int vEnd = LowerBound(vEdgeXs, x2);
                bool blocked = false;

                for (int k = vStart; k < vEnd && !blocked; k++)
                {
                    var e = vEdgesByX[k];
                    if (e.minY < y2 && e.maxY > y1) blocked = true;
                }

                if (blocked) continue;

                // Check horizontal edges crossing rectangle (y in (y1,y2), x overlaps [x1,x2])
                int hStart = LowerBound(hEdgeYs, y1 + 1);
                int hEnd = LowerBound(hEdgeYs, y2);

                for (int k = hStart; k < hEnd && !blocked; k++)
                {
                    var e = hEdgesByY[k];
                    if (e.minX < x2 && e.maxX > x1) blocked = true;
                }

                if (blocked) continue;

                // Ray cast to check if inside polygon
                int midX = (x1 + x2) / 2, midY = (y1 + y2) / 2;
                int crossings = 0;
                int rayStart = LowerBound(vEdgeXs, midX + 1);
                for (int k = rayStart; k < vEdgesByX.Length; k++)
                {
                    var e = vEdgesByX[k];
                    if (midY >= e.minY && midY < e.maxY) crossings++;
                }

                if (crossings % 2 == 1)
                {
                    long area = (long)(x2 - x1 + 1) * (y2 - y1 + 1);
                    maxArea = Math.Max(maxArea, area);
                }
            }
        }

        return maxArea;
    }

    private static int LowerBound(int[] arr, int value)
    {
        int lo = 0, hi = arr.Length;
        while (lo < hi)
        {
            int mid = (lo + hi) / 2;
            if (arr[mid] < value) lo = mid + 1;
            else hi = mid;
        }
        return lo;
    }
}
