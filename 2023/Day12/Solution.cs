using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day12;

[ProblemName("Hot Springs")]
internal class Solution : ISolver
{
    // caching pattern + group lengths to speed up processing on part 2
    private readonly Dictionary<string, long> cache = new();

    public (long, long) Solve(string input)
    {
        var lines = input.ReadLinesToType<string>().ToList();

        var part1 = 0L;
        var part2 = 0L;
        foreach (var line in lines.Select(x => x.Split(' ')))
        {
            var springs = line[0];
            var groups = line[1].Split(',').Select(int.Parse).ToList();

            part1 += Calculate(springs, groups);

            springs = string.Join('?', Enumerable.Repeat(springs, 5));
            groups = Enumerable.Repeat(groups, 5).SelectMany(g => g).ToList();

            part2 += Calculate(springs, groups);
        }


        return (part1, part2);
    }

    private long Calculate(string springs, List<int> groups)
    {
        var key = $"{springs},{string.Join(',', groups)}"; // Cache key: spring pattern + group lengths

        if (cache.TryGetValue(key, out var value)) return value;

        value = GetCount(springs, groups);
        cache[key] = value;

        return value;
    }


    private long GetCount(string springs, List<int> groups)
    {
        while (true)
        {
            if (groups.Count == 0)
                return springs.Contains('#')
                    ? 0
                    : 1; // No more groups to match: if there are no springs left, we have a match

            if (string.IsNullOrEmpty(springs))
                return 0; // No more springs to match, although we still have groups to match

            if (springs.StartsWith('.'))
            {
                springs = springs.Trim('.'); // Remove all dots from the beginning
                continue;
            }

            if (springs.StartsWith('?'))
                return Calculate("." + springs[1..], groups) +
                       Calculate("#" + springs[1..], groups); // Try both options recursively

            if (springs.StartsWith('#')) // Start of a group
            {
                if (springs.Length < groups[0]) return 0; // Not enough characters to match the group

                if (springs[..groups[0]].Contains('.')) return 0; // Group cannot contain dots for the given length

                if (groups.Count > 1)
                {
                    if (springs.Length < groups[0] + 1 ||
                        springs[groups[0]] ==
                        '#') return 0; // Group cannot be followed by a spring, and there must be enough characters left

                    springs = springs[
                        (groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                    groups = groups[1..];
                    continue;
                }

                springs = springs[groups[0]..]; // Last group, no need to check the character after the group
                groups = groups[1..];
                continue;
            }

            throw new Exception("Invalid input");
        }
    }
}