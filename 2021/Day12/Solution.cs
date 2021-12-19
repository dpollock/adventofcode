using adventofcode.Lib;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day12
{
    [ProblemName("Passage Pathing")]
    internal class Solution : ISolver
    {
        public object PartOne(string input)
        {
            return Explore(input, false);
        }

        public object PartTwo(string input)
        {
            return Explore(input, true);
        }

        private int Explore(string input, bool part2)
        {
            var map = GetMap(input);

            // Recursive approach.
            int PathCount(string currentCave, ImmutableHashSet<string> visitedCaves, bool anySmallCaveWasVisitedTwice)
            {
                if (currentCave == "end")
                {
                    return 1;
                }

                var res = 0;
                foreach (var cave in map[currentCave])
                {
                    var isBigCave = cave.ToUpper() == cave;
                    var seen = visitedCaves.Contains(cave);

                    if (!seen || isBigCave)
                    {
                        // we can visit big caves any number of times, small caves only once
                        res += PathCount(cave, visitedCaves.Add(cave), anySmallCaveWasVisitedTwice);
                    }
                    else if (part2 && cave != "start" && !anySmallCaveWasVisitedTwice)
                    {
                        // part 2 also lets us to visit a single small cave twice (except for start and end)
                        res += PathCount(cave, visitedCaves, true);
                    }
                }

                return res;
            }

            return PathCount("start", ImmutableHashSet.Create("start"), false);
        }

        private Dictionary<string, string[]> GetMap(string input)
        {
            // taking all connections:
            var connections =
                from line in input.ReadLinesToType<string>()
                let parts = line.Split("-")
                let caveA = parts[0]
                let caveB = parts[1]
                from connection in new[] { (From: caveA, To: caveB), (From: caveB, To: caveA) }
                select connection;

            return (
                from p in connections
                group p by p.From
                into g
                select g
            ).ToDictionary(g => g.Key, g => g.Select(connection => connection.To).ToArray());
        }
    }
}