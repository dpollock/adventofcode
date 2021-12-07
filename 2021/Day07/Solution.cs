using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day07
{
  [ProblemName("The Treachery of Whales")]      
  class Solution : ISolver {

        public object PartOne(string input) {

            var positions = input.Split(",").Select(int.Parse).ToList();

            int min = positions.Min();
            int max = positions.Max();

            int cheapest = int.MaxValue;
            for (int i = min; i < max; i++)
            {
                int totalCost = positions.Sum(p => Math.Abs(p - i));
                if (totalCost < cheapest)
                    cheapest = totalCost;
            }

            return cheapest;
        }

        public object PartTwo(string input) {
            var positions = input.Split(",").Select(int.Parse).ToList();

            int min = positions.Min();
            int max = positions.Max();

            int cheapest = int.MaxValue;
            for (int i = min; i < max; i++)
            {
                int totalCost = positions.Sum(p =>
                {
                    var n = Math.Abs(p - i);
                    return (n * (n+1))/2;
                });

                if (totalCost < cheapest)
                    cheapest = totalCost;
            }

            return cheapest;
}
  }
}
