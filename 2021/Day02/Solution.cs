using adventofcode.Lib;

namespace AdventOfCode.Y2021.Day02
{
    [ProblemName("Dive!")]
    internal class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var directions = input.ReadLinesToObject<InputLine>();

            var depth = 0;
            var horizontal = 0;

            foreach (var d in directions)
            {
                switch (d.Command)
                {
                    case Command.forward:
                        horizontal += d.Amount;
                        break;
                    case Command.down:
                        depth += d.Amount;
                        break;
                    case Command.up:
                        depth -= d.Amount;
                        break;
                }
            }

            return depth * horizontal;
        }

        public object PartTwo(string input)
        {
            var directions = input.ReadLinesToObject<InputLine>();

            var depth = 0;
            var horizontal = 0;
            var aim = 0;
            foreach (var d in directions)
                switch (d.Command)
                {
                    case Command.forward:
                        horizontal += d.Amount;
                        depth += d.Amount * aim;
                        break;
                    case Command.down:
                        aim += d.Amount;
                        break;
                    case Command.up:
                        aim -= d.Amount;
                        break;
                }

            return depth * horizontal;
        }

        private enum Command
        {
            forward,
            down,
            up
        }

        private class InputLine
        {
            public Command Command { get; set; }
            public int Amount { get; set; }
        }
    }
}