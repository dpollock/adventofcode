using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day05
{
    [ProblemName("If You Give A Seed A Fertilizer")]
    class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var (mapGroups, seeds) = ParseInput(input);

            var lowestLocation = long.MaxValue;
            foreach (var seed in seeds)
            {
                var currentSeed = seed;
                foreach (var map in mapGroups)
                {
                    var match = map.Value.FirstOrDefault(r =>
                        currentSeed >= r.Start && currentSeed <= r.End);
                    if (match != null)
                    {
                        currentSeed += match.Adjustment;
                    }
                }

                lowestLocation = Math.Min(currentSeed, lowestLocation);
            }

            return lowestLocation;
        }

        public object PartTwo(string input)
        {
            var (mapGroups, seeds) = ParseInput(input);


            var ranges = new List<(long start, long end)>();
            for (int i = 0; i < seeds.Count; i += 2)
                ranges.Add((start: seeds[i], end: seeds[i] + seeds[i + 1] - 1));


            foreach (var map in mapGroups)
            {
                var orderedMap = map.Value.OrderBy(x => x.Start).ToList();

                var newRanges = new List<(long start, long end)>();
                foreach (var r in ranges)
                {
                    var range = r;
                    foreach (var mapping in orderedMap)
                    {
                        if (range.start < mapping.Start)
                        {
                            newRanges.Add((range.start, Math.Min(range.end, mapping.Start - 1)));
                            range.start = mapping.Start;
                            if (range.start > range.end)
                                break;
                        }

                        if (range.start <= mapping.End)
                        {
                            newRanges.Add((range.start + mapping.Adjustment,
                                Math.Min(range.end, mapping.End) + mapping.Adjustment));
                            range.start = mapping.End + 1;
                            if (range.start > range.end)
                                break;
                        }
                    }

                    if (range.start <= range.end)
                        newRanges.Add(range);
                }

                ranges = newRanges;
            }

            var result2 = ranges.Min(r => r.start);

            return result2;
        }


        private static (Dictionary<string, List<SeedRange>> maps, List<long> seeds) ParseInput(string input)
        {
            List<long> seeds = new List<long>();
            var maps = new Dictionary<string, List<SeedRange>>();

            var mapGroups = input.ReadLinesToGroupsOfType<string>();
            foreach (var g in mapGroups)
            {
                if (g[0].StartsWith("seeds:"))
                {
                    seeds = g[0].Split(" ")[1..].Select(long.Parse).ToList();
                    continue;
                }

                var ranges = new List<SeedRange>();
                string currentMap = g[0].Split(" ")[0];

                foreach (var l in g[1..])
                {
                    var r = l.Split(" ");
                    ranges.Add(new SeedRange
                    {
                        Start = long.Parse(r[1]),
                        End = long.Parse(r[1]) + long.Parse(r[2]) - 1,
                        Adjustment = long.Parse(r[0]) - long.Parse(r[1])
                    });
                }

                maps.Add(currentMap, ranges);
            }

            return (maps, seeds);
        }
    }


    internal class SeedRange
    {
        public long Start { get; set; }
        public long End { get; set; }
        public long Adjustment { get; set; }
    }
}