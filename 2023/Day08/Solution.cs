using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day08
{
    [ProblemName("Haunted Wasteland")]
    class Solution : ISolver
    {
        public object PartOne(string input)
        {
            return CountSteps(input, true);
        }

        private static long CountSteps(string input, bool part1)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var instructions = lines[0].ToCharArray();

            var map = ParseNodes(lines);

            var endingInA = part1
                ? map.Where(kvp => kvp.Key == "AAA").Select(kvp => kvp.Key).ToList()
                : map.Where(kvp => kvp.Key.EndsWith('A')).Select(kvp => kvp.Key).ToList();

            List<long> stepsList = new();
            foreach (var node in endingInA)
            {
                long steps = 0;
                var currentNode = node;
                for (; steps % instructions.Length < instructions.Length; steps++)
                {
                    if (currentNode.EndsWith('Z'))
                    {
                        break;
                    }

                    currentNode = instructions[(int)steps % instructions.Length] == 'L'
                        ? map[currentNode].left
                        : map[currentNode].right;
                }

                stepsList.Add(steps);
            }

            var count = stepsList.Aggregate(LCM);
            return count;
        }

        public object PartTwo(string input)
        {
            return CountSteps(input, false);
        }

        private static Dictionary<string, (string left, string right)> ParseNodes(List<string> lines)
        {
            // Regex to parse XKM = (FRH, RLM) 
            var regex = new Regex(@"(\w+) = (\((\w+), (\w+)\))");
            var dict = new Dictionary<string, (string left, string right)>();
            foreach (var step in lines[2..])
            {
                var match = regex.Match(step);
                dict.Add(match.Groups[1].Value, (match.Groups[3].Value, match.Groups[4].Value));
            }

            return dict;
        }


        private static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        private static long LCM(long a, long b)
        {
            return (a / GCD(a, b)) * b;
        }
    }
}