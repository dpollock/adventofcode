namespace AdventOfCode.Y2025;

public class Day08() : Solver(2025, 8, "Playground")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
       ("""
       162,817,812
        57,618,57
        906,360,560
        592,479,940
        352,342,300
        466,668,158
        542,29,236
        431,825,988
        739,650,466
        52,470,668
        216,146,977
        819,987,18
        117,168,530
        805,96,715
        346,949,466
        970,615,88
        941,993,340
        862,61,35
        984,92,344
        425,690,689
""", 40, 25272),  // Part 2 expected not yet known
    ];

    public override long Part1(string input)
    {
        var points = Parse.Lines(input)
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(p => (x: p[0], y: p[1], z: p[2]))
            .ToList();

        // Sample uses 10 connections, real input uses 1000
        int connections = points.Count == 20 ? 10 : 1000;
        return Solve(points, connections);
    }

    public override long Part2(string input)
    {
        var points = Parse.Lines(input)
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(p => (x: p[0], y: p[1], z: p[2]))
            .ToList();

        // Part 2: Find total cable length to connect all into one circuit (MST)
        return SolvePart2(points);
    }

    private static long SolvePart2(List<(int x, int y, int z)> points)
    {
        int n = points.Count;

        // Generate all pairs with their squared distances
        var pairs = new List<(int i, int j, long distSq)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = points[i].x - points[j].x;
                long dy = points[i].y - points[j].y;
                long dz = points[i].z - points[j].z;
                pairs.Add((i, j, dx * dx + dy * dy + dz * dz));
            }
        }

        // Sort by distance
        pairs.Sort((a, b) => a.distSq.CompareTo(b.distSq));

        // Union-Find
        int[] parent = new int[n];
        int[] rank = new int[n];
        for (int i = 0; i < n; i++)
            parent[i] = i;

        int Find(int x)
        {
            if (parent[x] != x)
                parent[x] = Find(parent[x]);
            return parent[x];
        }

        bool Union(int x, int y)
        {
            int px = Find(x), py = Find(y);
            if (px == py) return false;
            if (rank[px] < rank[py]) (px, py) = (py, px);
            parent[py] = px;
            if (rank[px] == rank[py]) rank[px]++;
            return true;
        }

        // Build MST - find the LAST connection that forms one circuit
        int edgesUsed = 0;
        int lastI = -1, lastJ = -1;
        foreach (var (i, j, _) in pairs)
        {
            if (Union(i, j))
            {
                lastI = i;
                lastJ = j;
                edgesUsed++;
                if (edgesUsed == n - 1) break;  // All connected
            }
        }

        // Return product of X coordinates of the last two connected junction boxes
        return (long)points[lastI].x * points[lastJ].x;
    }

    private static long Solve(List<(int x, int y, int z)> points, int connections)
    {
        int n = points.Count;

        // Generate all pairs with their squared distances
        var pairs = new List<(int i, int j, long distSq)>();
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                long dx = points[i].x - points[j].x;
                long dy = points[i].y - points[j].y;
                long dz = points[i].z - points[j].z;
                pairs.Add((i, j, dx * dx + dy * dy + dz * dz));
            }
        }

        // Sort by distance
        pairs.Sort((a, b) => a.distSq.CompareTo(b.distSq));

        // Union-Find with path compression and union by rank
        int[] parent = new int[n];
        int[] rank = new int[n];
        int[] size = new int[n];
        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
            size[i] = 1;
        }

        int Find(int x)
        {
            if (parent[x] != x)
                parent[x] = Find(parent[x]);
            return parent[x];
        }

        void Union(int x, int y)
        {
            int px = Find(x), py = Find(y);
            if (px == py) return;
            if (rank[px] < rank[py]) (px, py) = (py, px);
            parent[py] = px;
            size[px] += size[py];
            if (rank[px] == rank[py]) rank[px]++;
        }

        // Make connections
        int made = 0;
        foreach (var (i, j, _) in pairs)
        {
            if (made >= connections) break;
            Union(i, j);
            made++;
        }

        // Get circuit sizes
        var circuitSizes = new List<int>();
        for (int i = 0; i < n; i++)
        {
            if (Find(i) == i)
                circuitSizes.Add(size[i]);
        }

        circuitSizes.Sort((a, b) => b.CompareTo(a));
        return (long)circuitSizes[0] * circuitSizes[1] * circuitSizes[2];
    }
}