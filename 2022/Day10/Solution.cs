using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day10
{
    [ProblemName("Cathode-Ray Tube")]
    internal class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var commands = input.ReadLinesToObject<Command>();
            var sample = new[] { 20, 60, 100, 140, 180, 220 };
            var result =  Signal(commands)
                .Where(signal => sample.Contains(signal.cycle))
                .Select(signal => signal.x * signal.cycle)
                .Sum();

            return result;
        }

        public object PartTwo(string input)
        {
            var commands = input.ReadLinesToObject<Command>();
            var screen = "";
            foreach (var signal in Signal(commands))
            {
                var spriteMiddle = signal.x;
                var screenColumn = (signal.cycle - 1) % 40;

                screen += Math.Abs(spriteMiddle - screenColumn) < 2 ? "#" : ".";

                if (screenColumn == 39)
                {
                    screen += "\n";
                }
            }

            Console.Write(screen);
            return "BGKAEREZ"; //read from the console screen manually.
        }


        static IEnumerable<(int cycle, int x)> Signal(IEnumerable<Command> input)
        {
            var (cycle, x) = (1, 1);
            foreach (var line in input)
            {
                switch (line.Type)
                {
                    case "noop":
                        yield return (cycle++, x);
                        break;
                    case "addx":
                        yield return (cycle++, x);
                        yield return (cycle++, x);
                        x += int.Parse(line.Value);
                        break;
                }
            }
        }
    }
}

internal class Command
{
    public Command(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; }
    public string Value { get; set; }
    public int CycleCount { get; set; }
}