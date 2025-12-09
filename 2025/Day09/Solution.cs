namespace AdventOfCode.Y2025;

public class Day09() : Solver(2025, 9, "")
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

    public override long Part1(string input)
    {
        var lines = Parse.Lines(input);
        (int x, int y)[] xyPairs = lines.Select(line => line.Split(',').Select(int.Parse).ToArray()).Select(p => (x: p[0], y: p[1])).ToList().ToArray();

        long largestRectangle = 0;
        for (int i = 0; i < xyPairs.Length; i++)
        {
            for (int j = i + 1; j < xyPairs.Length; j++)
            {
                long width = Math.Abs(xyPairs[i].x - xyPairs[j].x) + 1;
                long height = Math.Abs(xyPairs[i].y - xyPairs[j].y) + 1;
                var area = width * height;
                largestRectangle = Math.Max(largestRectangle, area);
            }
        }

        return largestRectangle;
    }

    public override long Part2(string input)
    {
        var lines = Parse.Lines(input);
        var vertices = lines.Select(line => line.Split(',').Select(int.Parse).ToArray())
                           .Select(p => (x: p[0], y: p[1])).ToArray();

        // Build edges list
        var edges = new List<(int x1, int y1, int x2, int y2)>();
        for (int i = 0; i < vertices.Length; i++)
        {
            var from = vertices[i];
            var to = vertices[(i + 1) % vertices.Length];
            edges.Add((from.x, from.y, to.x, to.y));
        }

        // For a rectangle to be fully inside polygon:
        // All 4 corners must be inside or on boundary, AND
        // No edge of polygon can cross through the rectangle interior

        // Since corners must be red (vertices), we check pairs of vertices
        // A rectangle is valid if all 4 edges of rectangle don't cross polygon edges
        // except at the corners themselves

        long largestRectangle = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i + 1; j < vertices.Length; j++)
            {
                var v1 = vertices[i];
                var v2 = vertices[j];

                int x1 = Math.Min(v1.x, v2.x);
                int x2 = Math.Max(v1.x, v2.x);
                int y1 = Math.Min(v1.y, v2.y);
                int y2 = Math.Max(v1.y, v2.y);

                if (x1 == x2 || y1 == y2) continue; // Skip degenerate rectangles

                // Check if rectangle is fully inside the polygon
                // by checking if any polygon edge crosses the rectangle interior
                bool valid = true;

                foreach (var (ex1, ey1, ex2, ey2) in edges)
                {
                    // Vertical edge
                    if (ex1 == ex2)
                    {
                        int edgeX = ex1;
                        int edgeMinY = Math.Min(ey1, ey2);
                        int edgeMaxY = Math.Max(ey1, ey2);

                        // Does this vertical edge pass through interior of rectangle?
                        if (edgeX > x1 && edgeX < x2)
                        {
                            // Edge x is strictly inside rectangle's x range
                            // Check if edge's y range overlaps rectangle's y range
                            if (edgeMinY < y2 && edgeMaxY > y1)
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                    // Horizontal edge
                    else
                    {
                        int edgeY = ey1;
                        int edgeMinX = Math.Min(ex1, ex2);
                        int edgeMaxX = Math.Max(ex1, ex2);

                        // Does this horizontal edge pass through interior of rectangle?
                        if (edgeY > y1 && edgeY < y2)
                        {
                            // Edge y is strictly inside rectangle's y range
                            // Check if edge's x range overlaps rectangle's x range
                            if (edgeMinX < x2 && edgeMaxX > x1)
                            {
                                valid = false;
                                break;
                            }
                        }
                    }
                }

                if (valid)
                {
                    // Also need to verify the rectangle center is inside the polygon
                    int midX = (x1 + x2) / 2;
                    int midY = (y1 + y2) / 2;

                    int crossings = 0;
                    foreach (var (ex1, ey1, ex2, ey2) in edges)
                    {
                        if (ex1 == ex2 && ex1 > midX) // vertical edge to the right
                        {
                            int edgeMinY = Math.Min(ey1, ey2);
                            int edgeMaxY = Math.Max(ey1, ey2);
                            if (midY >= edgeMinY && midY < edgeMaxY)
                                crossings++;
                        }
                    }

                    if (crossings % 2 == 1) // inside polygon
                    {
                        long width = x2 - x1 + 1;
                        long height = y2 - y1 + 1;
                        long area = width * height;
                        largestRectangle = Math.Max(largestRectangle, area);
                    }
                }
            }
        }

        return largestRectangle;
    }
}