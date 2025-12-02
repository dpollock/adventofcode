namespace AdventOfCode.Y2025;

public class Day01() : Solver(2025, 1, "Secret Entrance")
{
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
}
