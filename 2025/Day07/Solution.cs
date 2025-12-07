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
        """, 21, null),
    ];

    public override long Part1(string input)
    {
        var lines = Parse.Lines(input);
        var height = lines.Length;
        var width = lines[0].Length;

        // Find starting position S
        var startCol = lines[0].IndexOf('S');

        // Track beams: positions where beams are currently active
        // We only care about unique positions (not counts), because merged beams stay merged
        var beamPositions = new HashSet<int> { startCol };
        long totalSplits = 0;

        for (var row = 1; row < height; row++)
        {
            var newBeamPositions = new HashSet<int>();

            foreach (var col in beamPositions)
            {
                var cell = lines[row][col];

                if (cell == '^')
                {
                    // Splitter: beam splits left and right
                    totalSplits++;

                    var leftCol = col - 1;
                    var rightCol = col + 1;

                    if (leftCol >= 0)
                        newBeamPositions.Add(leftCol);
                    if (rightCol < width)
                        newBeamPositions.Add(rightCol);
                }
                else
                {
                    // Empty space: beam continues downward
                    newBeamPositions.Add(col);
                }
            }

            beamPositions = newBeamPositions;
        }

        return totalSplits;
    }

    public override long Part2(string input)
    {
        var lines = Parse.Lines(input);

        return -1;
    }
}