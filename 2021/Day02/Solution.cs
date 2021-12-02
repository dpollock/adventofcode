using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day02
{
  [ProblemName("Dive!")]      
  class Solution : ISolver {

        public object PartOne(string input)
        {
            var directions = input.ReadLines<string>().ToList();

            var depth = 0;
            var horizontal = 0;

            foreach (var direction in directions)
            {
                var parsed  =direction.Split(" ");
                if (parsed[0] == "forward")
                {
                    horizontal += int.Parse(parsed[1]);
                }
                if (parsed[0] == "down")
                {
                    depth += int.Parse(parsed[1]);
                }

                if (parsed[0] == "up")
                {
                    depth -= int.Parse(parsed[1]);
                }
            }

            return depth * horizontal;
        }

        public object PartTwo(string input) {
            var directions = input.ReadLines<string>().ToList();

            var depth = 0;
            var horizontal = 0;
            var aim = 0;
            foreach (var direction in directions)
            {
                var parsed = direction.Split(" ");
                if (parsed[0] == "forward")
                {
                    horizontal += int.Parse(parsed[1]);
                    depth += int.Parse(parsed[1]) * aim;
                }
                if (parsed[0] == "down")
                {
                    aim += int.Parse(parsed[1]);
                }

                if (parsed[0] == "up")
                {
                    aim -= int.Parse(parsed[1]);
                }
            }

            return depth * horizontal;
}
  }
}
