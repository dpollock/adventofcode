using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using adventofcode.Lib;
using SuperLinq;

namespace AdventOfCode.Y2023.Day06
{
    [ProblemName("Wait For It")]
    partial class Solution : ISolver
    {

        public object PartOne(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var times = NumberRegex().Matches(lines[0].Split(":")[1]).Select(x => long.Parse(x.Value)).ToList();
            var distances = NumberRegex().Matches(lines[1].Split(":")[1]).Select(x => long.Parse(x.Value)).ToList();

            long sum = 1;
            for (int i = 0; i < times.Count(); i++)
            {
                //distance = timeHeld  * (totalTime - timeHeld)
                // x^2 + Tx + D = 0
                // where T is the time or the race and D is the distance of the record for that race
                // quadratic formula
                // and x = is the time to hold the button down
                // x = (T + sqrt(T^2 - 4D)) / 2
                // x = (T - sqrt(T^2 - 4D)) / 2

                var raceTime = times[i];
                var record = distances[i]; // gotta beat the distance by at least 1, right?

                double low = (raceTime - Math.Sqrt(raceTime * raceTime - 4 * record)) / 2;
                double high = (raceTime + Math.Sqrt(raceTime * raceTime - 4 * record)) / 2;
                var holdCount = Math.Ceiling(high - 1) - Math.Floor(low + 1) + 1;

                sum *= (long)holdCount;
            }


            return sum;
        }

        public object PartTwo(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var time = long.Parse(string.Concat(NumberRegex().Matches(lines[0].Split(":")[1]).Select(x => x.Value)));
            var distance = long.Parse(string.Concat(NumberRegex().Matches(lines[1].Split(":")[1]).Select(x => x.Value)));

            var raceTime = time;
            var record = distance; // gotta beat the distance by at least 1, right?

            double low = (raceTime - Math.Sqrt(raceTime * raceTime - 4 * record)) / 2;
            double high = (raceTime + Math.Sqrt(raceTime * raceTime - 4 * record)) / 2;
            var holdCount = Math.Ceiling(high - 1) - Math.Floor(low + 1) + 1;

            return holdCount;
        }

        [GeneratedRegex(@"(\d+)")]
        private static partial Regex NumberRegex();
    }

}
