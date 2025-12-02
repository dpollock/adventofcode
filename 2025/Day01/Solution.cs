namespace AdventOfCode.Y2025;

public class Day01() : Solver(2025, 1, "Secret Entrance")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        L68
        L30
        R48
        L5
        R60
        L55
        L1
        L99
        R14
        L82
        """, 3, 6)
    ];

    public override long Part1(string input)
    {
        var dial = 50;
        var timesOnZero = 0;

        var moves = Parse.Lines(input, line => (line[0], int.Parse(line[1..])));
        foreach (var (direction, distance) in moves)
        {
            dial = direction == 'L'
                ? (dial - distance % 100 + 100) % 100
                : (dial + distance) % 100;

            if (dial == 0) timesOnZero++;
        }

        return timesOnZero;
    }

    public override long Part2(string input)
    {
        var dial = 50;
        var timesOnZero = 0L;

        var moves = Parse.Lines(input, line => (line[0], int.Parse(line[1..])));
        foreach (var (direction, distance) in moves)
        {
            // Count how many times we land on 0 during this rotation
            // Each click moves us one position; we count each time we're at 0
            if (direction == 'L')
            {
                // Going left from dial by distance clicks
                // We visit 0 if dial >= 0 and dial - distance < 0 (wrapping)
                // Or if we do multiple full rotations
                // Positions visited: dial, dial-1, dial-2, ... dial-distance (mod 100)
                // We hit 0 when (dial - k) % 100 == 0, for k in 1..distance
                // That's when k = dial, dial+100, dial+200, etc. and k <= distance
                if (distance >= dial && dial > 0)
                    timesOnZero += 1 + (distance - dial) / 100;
                else if (dial == 0)
                    timesOnZero += distance / 100;
                dial = ((dial - distance) % 100 + 100) % 100;
            }
            else
            {
                // Going right from dial by distance clicks
                // We hit 0 when (dial + k) % 100 == 0, for k in 1..distance
                // That's when k = 100-dial, 200-dial, etc. and k <= distance
                var firstZero = (100 - dial) % 100;
                if (firstZero == 0) firstZero = 100;
                if (distance >= firstZero)
                    timesOnZero += 1 + (distance - firstZero) / 100;
                dial = (dial + distance) % 100;
            }
        }

        return timesOnZero;
    }
}
