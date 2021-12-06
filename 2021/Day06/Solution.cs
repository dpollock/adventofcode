using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day06
{
    [ProblemName("Lanternfish")]
    internal class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var fishes = input.Split(",").Select(ulong.Parse).ToList();

            var days = 80;

            GrowFish(days, fishes);


            var count = fishes.Count();
            return count;
        }

        public object PartTwo(string input)
        {
            var fishes = input.Split(",").Select(ulong.Parse).ToList();

            var days = 256;

            GrowFish(days, fishes);


            var count = fishes.Count();
            return count;

        }

        private static void GrowFish(int days, List<ulong> fishes)
        {
            for (var i = 0; i < days; i++)
            {
                var currFish = fishes.Count;
                for (var j = 0; j < currFish; j++)
                    if (fishes[j] == 0)
                    {
                        fishes[j] = 6;
                        fishes.Add(8);
                    }
                    else
                    {
                        fishes[j]--;
                    }
            }
        }
    }
}