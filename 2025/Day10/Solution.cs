namespace AdventOfCode.Y2025;

public class Day10() : Solver(2025, 10, "Factory")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
        [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
        """, 7, null),
    ];

    public override long Part1(string input)
    {
        var lines = Parse.Lines(input);
        long total = 0;

        foreach (var line in lines)
        {
            var (target, buttons) = ParseMachine(line);
            total += FindMinPresses(target, buttons);
        }

        return total;
    }

    public override long Part2(string input)
    {
        var lines = Parse.Lines(input);

        return -1;
    }

    private (int target, int[] buttons) ParseMachine(string line)
    {
        // Parse indicator light diagram [.##.]
        var bracketStart = line.IndexOf('[');
        var bracketEnd = line.IndexOf(']');
        var diagram = line[(bracketStart + 1)..bracketEnd];

        int target = 0;
        for (int i = 0; i < diagram.Length; i++)
        {
            if (diagram[i] == '#')
                target |= (1 << i);
        }

        // Parse button wiring schematics (x,y,z)
        var buttons = new List<int>();
        int pos = bracketEnd + 1;

        while (pos < line.Length)
        {
            var parenStart = line.IndexOf('(', pos);
            if (parenStart == -1) break;

            var parenEnd = line.IndexOf(')', parenStart);
            var content = line[(parenStart + 1)..parenEnd];

            int buttonMask = 0;
            foreach (var numStr in content.Split(','))
            {
                int lightIndex = int.Parse(numStr.Trim());
                buttonMask |= (1 << lightIndex);
            }
            buttons.Add(buttonMask);

            pos = parenEnd + 1;

            // Stop when we hit the curly braces (joltage requirements)
            if (pos < line.Length && line[pos..].TrimStart().StartsWith('{'))
                break;
        }

        return (target, buttons.ToArray());
    }

    private int FindMinPresses(int target, int[] buttons)
    {
        int n = buttons.Length;
        int minPresses = int.MaxValue;

        // Try all subsets of buttons (each button pressed 0 or 1 times)
        for (int mask = 0; mask < (1 << n); mask++)
        {
            int state = 0;
            int presses = 0;

            for (int i = 0; i < n; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    state ^= buttons[i];
                    presses++;
                }
            }

            if (state == target && presses < minPresses)
            {
                minPresses = presses;
            }
        }

        return minPresses == int.MaxValue ? 0 : minPresses;
    }
}