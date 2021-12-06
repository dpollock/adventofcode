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

            //instead of keep track of each individual fish which will make us quickly hit the ulong limit and take FOREVER,
            //we'll keep track of each day and each run will just "rotate" the days numbers. Less looping, more efficent
            var fishes = input.Split(",").Select(int.Parse)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            var days = 256;
            for (int i = 0; i < days; i++)
                fishes = GrowFishPart2(fishes);



            var count = fishes.Values.Sum();
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

        private static Dictionary<int, long> GrowFishPart2(Dictionary<int, long> fishes) =>
            new()
            {
                // 8 is "new fish" and all zeros move to that
                [8] = fishes.GetValueOrDefault(0),
                [7] = fishes.GetValueOrDefault(8),
                // 0 ages go to 6, along with 7 ages
                [6] = fishes.GetValueOrDefault(0) + fishes.GetValueOrDefault(7),
                [5] = fishes.GetValueOrDefault(6),
                [4] = fishes.GetValueOrDefault(5),
                [3] = fishes.GetValueOrDefault(4),
                [2] = fishes.GetValueOrDefault(3),
                [1] = fishes.GetValueOrDefault(2),
                [0] = fishes.GetValueOrDefault(1),
            };
    }
}