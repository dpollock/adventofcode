using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day06
{
    [ProblemName("Lanternfish")]
    class Solution : ISolver
    {

        public object PartOne(string input)
        {

            var fishes = input.Split(",").Select(int.Parse).ToList();

            var days = 80;

            for (int i = 0; i < days; i++)
            {

                var currFish = fishes.Count;
                for (int j = 0; j < currFish; j++)
                {
                    if (fishes[j] == 0)
                    {
                        fishes[j] = 6 ;
                        fishes.Add(8);

                    }
                    else
                    {
                        fishes[j]--;
                    }

                }

            }



            var count = fishes.Count();
            return  count;
        }

        public object PartTwo(string input)
        {
            return 0;
        }
    }
}
