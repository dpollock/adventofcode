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
        """, 50, null),
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

        return -1;
    }
}