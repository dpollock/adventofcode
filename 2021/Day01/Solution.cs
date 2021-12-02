using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day01
{
    [ProblemName("Sonar Sweep")]
    internal class Solution : ISolver
    {
        public object PartOne(string input)
        {
            return CountNumOfIncreases(input.ReadLines<int>().ToList());
        }

        public object PartTwo(string input)
        {
            var allInput = input.ReadLines<int>().ToList();

            var threeGroupSums = allInput.Skip(2).Select((x, index) => x + allInput[index] + allInput[index + 1]).ToList();

            return CountNumOfIncreases(threeGroupSums);
        }

        private static object CountNumOfIncreases(List<int> numbers)
        {
            var numOfIncreases = 0;
            var previous = 0;
            foreach (var current in numbers)
            {
                if (previous != 0 && current > previous)
                {
                    numOfIncreases++;
                }

                previous = current;
            }

            return numOfIncreases;
        }
    }
}