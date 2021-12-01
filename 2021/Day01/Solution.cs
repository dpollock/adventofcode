using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day01
{
  [ProblemName("Sonar Sweep")]      
  class Solution : ISolver {

        public object PartOne(string input)
        {
            var numOfIncreases = 0;
            int previous = 0;
            foreach (var current in input.ReadLines<int>())
            {
                if (previous != 0 && current > previous)
                {
                    numOfIncreases++;
                }

                previous = current;
            }
            return numOfIncreases;
        }

        public object PartTwo(string input) {
            return 0;
     }
  }
}
