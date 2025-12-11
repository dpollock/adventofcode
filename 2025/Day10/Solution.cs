using System.Numerics;

namespace AdventOfCode.Y2025;

public class Day10() : Solver(2025, 10, "Factory")
{
    public override (string input, long? expected1, long? expected2)[] Samples =>
    [
        ("""
        [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
        [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
        [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
        """, 7, 33),
    ];

    public override long Part1(string input)
    {
        long total = 0;
        foreach (var line in Parse.Lines(input))
        {
            var (target, buttons, _) = ParseMachine(line);
            total += FindMinPresses(target, buttons);
        }
        return total;
    }

    public override long Part2(string input)
    {
        long total = 0;
        foreach (var line in Parse.Lines(input))
        {
            var (_, buttons, joltages) = ParseMachine(line);
            total += FindMinPressesJoltage(buttons, joltages);
        }
        return total;
    }

    // Unified parser: returns (targetBitmask, buttonIndices[][], joltages[])
    private static (int target, int[][] buttons, int[] joltages) ParseMachine(string line)
    {
        // Parse indicator light diagram [.##.]
        var bracketEnd = line.IndexOf(']');
        int target = 0;
        for (int i = 1; i < bracketEnd; i++)
            if (line[i] == '#')
                target |= 1 << (i - 1);

        // Parse button wiring schematics (x,y,z)
        var buttons = new List<int[]>();
        int pos = bracketEnd + 1;
        var curlyStart = line.IndexOf('{');

        while (pos < curlyStart)
        {
            var parenStart = line.IndexOf('(', pos);
            if (parenStart == -1 || parenStart > curlyStart) break;

            var parenEnd = line.IndexOf(')', parenStart);
            var indices = line[(parenStart + 1)..parenEnd]
                .Split(',')
                .Select(int.Parse)
                .ToArray();
            buttons.Add(indices);
            pos = parenEnd + 1;
        }

        // Parse joltage requirements {x,y,z}
        var curlyEnd = line.IndexOf('}');
        var joltages = line[(curlyStart + 1)..curlyEnd]
            .Split(',')
            .Select(int.Parse)
            .ToArray();

        return (target, buttons.ToArray(), joltages);
    }

    private static int FindMinPresses(int target, int[][] buttons)
    {
        int n = buttons.Length;

        // Pre-compute bitmasks for each button
        var masks = new int[n];
        for (int i = 0; i < n; i++)
            foreach (var idx in buttons[i])
                masks[i] |= 1 << idx;

        int minPresses = int.MaxValue;

        // Try all subsets of buttons (each button pressed 0 or 1 times)
        for (int mask = 0; mask < (1 << n); mask++)
        {
            int state = 0;
            for (int i = 0; i < n; i++)
                if ((mask & (1 << i)) != 0)
                    state ^= masks[i];

            if (state == target)
                minPresses = Math.Min(minPresses, BitOperations.PopCount((uint)mask));
        }

        return minPresses == int.MaxValue ? 0 : minPresses;
    }

    private static long FindMinPressesJoltage(int[][] buttons, int[] target)
    {
        int R = target.Length;
        int C = buttons.Length;

        // Create augmented matrix: A[r][c] = 1 if button c affects counter r
        var A = new int[R][];
        for (int r = 0; r < R; r++)
        {
            A[r] = new int[C + 1];
            A[r][C] = target[r];
        }

        for (int b = 0; b < C; b++)
            foreach (var counter in buttons[b])
                if (counter < R)
                    A[counter][b] = 1;

        // Gaussian elimination using integer arithmetic
        for (int r = 0; r < R && r < C; r++)
        {
            // Find pivot
            int pivot = -1;
            for (int p = r; p < R; p++)
                if (A[p][r] != 0) { pivot = p; break; }

            if (pivot == -1) continue;

            (A[pivot], A[r]) = (A[r], A[pivot]);

            // Reduce rows below pivot
            for (int p = r + 1; p < R; p++)
            {
                if (A[p][r] == 0) continue;

                int num = A[p][r], den = A[r][r];
                for (int c = 0; c <= C; c++)
                    A[p][c] = den * A[p][c] - A[r][c] * num;
            }
        }

        // Pre-compute max presses per button
        var maximums = new int[C];
        for (int b = 0; b < C; b++)
        {
            maximums[b] = int.MaxValue;
            foreach (var counter in buttons[b])
                if (counter < R)
                    maximums[b] = Math.Min(maximums[b], target[counter]);
            if (maximums[b] == int.MaxValue) maximums[b] = 0;
        }

        // Back substitution with brute force
        int best = int.MaxValue;
        var pressed = new int[C];
        Array.Fill(pressed, -1);
        BackSubRecursive(A, C, maximums, pressed, R - 1, 0, ref best);
        return best;
    }

    private static void BackSubRecursive(int[][] A, int C, int[] maximums, int[] pressed, int r, int currentSum, ref int best)
    {
        if (currentSum >= best) return;

        if (r < 0)
        {
            best = currentSum;
            return;
        }

        long rowTotal = A[r][C];
        int firstUnknown = -1;

        for (int c = 0; c < C; c++)
        {
            if (A[r][c] != 0)
            {
                if (pressed[c] >= 0)
                    rowTotal -= (long)A[r][c] * pressed[c];
                else if (firstUnknown == -1)
                    firstUnknown = c;
            }
        }

        if (firstUnknown == -1)
        {
            if (rowTotal == 0)
                BackSubRecursive(A, C, maximums, pressed, r - 1, currentSum, ref best);
            return;
        }

        for (int p = 0; p <= maximums[firstUnknown] && currentSum + p < best; p++)
        {
            pressed[firstUnknown] = p;
            BackSubRecursive(A, C, maximums, pressed, r, currentSum + p, ref best);
            pressed[firstUnknown] = -1;
        }
    }
}