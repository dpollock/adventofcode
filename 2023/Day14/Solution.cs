using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day14
{
    [ProblemName("Parabolic Reflector Dish")]
    class Solution : ISolver
    {

        public (long, long) Solve(string input)
        {
            var part1 = TotalLoad(Tilt(input.ToCharGrid(), Direction.North));
            var part2 = TotalLoad(TiltCycles(1_000_000_000, input.ToCharGrid()));
            return (part1, part2);
        }

        public (long, long, long, long) SolveSample(string input)
        {
            var sample =
                """
                O....#....
                O.OO#....#
                .....##...
                OO.#O....O
                .O.....O#.
                O.#..O.#.#
                ..O..#O..O
                .......O..
                #....###..
                #OO..#....
                """;

            var part1 = TotalLoad(Tilt(sample.ToCharGrid(), Direction.North));
            var part2 = TotalLoad(TiltCycles(1_000_000_000, sample.ToCharGrid()));

            return (part1, part2, 136, 64);
        }


        static int TotalLoad(char[][] platform)
              => platform.Select((row, y) => row.Count(c => c == 'O') * (platform.Length - y)).Sum();

        static char[][] TiltCycles(int totalCycles, char[][] platform)
        {
            // because total cycles could be very large, at a certain point a lot of stuff doesn't change, let's cache it.
            var seen = new Dictionary<long, int>();

            for (var cycle = 1; cycle <= totalCycles; cycle++)
            {
                DoCycle();

                var key = Hash(platform);
                if (seen.TryGetValue(key, out int cycleStart))
                {
                    var cycleLength = cycle - cycleStart;
                    var remainingCycles = (totalCycles - cycleStart) % cycleLength;
                    for (var i = 0; i < remainingCycles; i++) DoCycle();
                    return platform;
                }
                else
                {
                    seen.Add(key, cycle);
                }
            }

            return platform;

            void DoCycle()
            {
                Tilt(platform, Direction.North);
                Tilt(platform, Direction.West);
                Tilt(platform, Direction.South);
                Tilt(platform, Direction.East);
            }
        }

        static long Hash(char[][] platform)
                => platform.Select((row, y) => row.Select((c, x) => c * (long)x).Sum() * y).Sum();


        static char[][] Tilt(char[][] platform, Direction direction)
        {
            // fancy way of looping dynamically through a 2x2 grid in different directions
            var (dy, dx, yStart, yEnd, yStep, xStart, xEnd, xStep) = direction switch
            {
                Direction.North => (-1, +0, +1, platform.Length, +1, 0, platform[0].Length, +1),
                Direction.South => (+1, +0, platform.Length - 2, -1, -1, 0, platform[0].Length, +1),

                Direction.West => (+0, -1, 0, platform.Length, +1, 1, platform[0].Length, +1),
                Direction.East => (+0, +1, 0, platform.Length, +1, platform[0].Length - 2, -1, -1),

                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            for (var y = yStart; y != yEnd; y += yStep)
                for (var x = xStart; x != xEnd; x += xStep)
                {
                    if (platform[y][x] != 'O') continue;

                    var y2 = y + dy;
                    var x2 = x + dx;
                    while (y2 >= 0 && y2 < platform.Length
                        && x2 >= 0 && x2 < platform[0].Length
                        && platform[y2][x2] == '.')
                    {
                        platform[y2][x2] = 'O';
                        platform[y2 - dy][x2 - dx] = '.';
                        y2 += dy;
                        x2 += dx;
                    }
                }

            return platform;
        }
    }
}
