using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2024.Day05
{
  [ProblemName("Print Queue")]
  class Solution : ISolver
  {
    private class Rule
    {
      public int Before { get; set; }
      public int After { get; set; }
    }

    private bool IsValidOrder(List<int> pages, Dictionary<int, HashSet<int>> dependencies)
    {
      for (int i = 0; i < pages.Count; i++)
      {
        var current = pages[i];
        if (dependencies.ContainsKey(current))
        {
          // Check if any required "before" pages come after this one
          var laterPages = new HashSet<int>(pages.Skip(i + 1));
          if (dependencies[current].Overlaps(laterPages))
            return false;
        }
      }
      return true;
    }

    private Dictionary<int, HashSet<int>> BuildDependencyGraph(List<Rule> rules)
    {
      var dependencies = new Dictionary<int, HashSet<int>>();

      foreach (var rule in rules)
      {
        // rule.Before must come before rule.After
        if (!dependencies.ContainsKey(rule.After))
        {
          dependencies[rule.After] = new HashSet<int>();
        }
        dependencies[rule.After].Add(rule.Before);
      }

      return dependencies;
    }

    private List<int> GetCorrectOrder(List<int> pages, Dictionary<int, HashSet<int>> dependencies)
    {
      var result = pages.ToList();
      bool madeSwap;

      // Bubble sort with dependencies
      do
      {
        madeSwap = false;
        for (int i = 0; i < result.Count - 1; i++)
        {
          var current = result[i];
          var next = result[i + 1];

          // If next must come before current, swap them
          if (dependencies.ContainsKey(next) && dependencies[next].Contains(current))
          {
            result[i] = next;
            result[i + 1] = current;
            madeSwap = true;
          }
        }
      } while (madeSwap);

      return result;
    }

    public (long, long) Solve(string input)
    {
      var groups = input.ReadLinesToGroupsOfType<string>().ToList();

      var rules = groups[0]
          .Select(line =>
          {
            var parts = line.Split('|');
            return new Rule
            {
              Before = int.Parse(parts[0]),
              After = int.Parse(parts[1])
            };
          })
          .ToList();

      var updates = groups[1]
          .Select(line => line.Split(',').Select(int.Parse).ToList())
          .ToList();

      // Build dependency graph
      var dependencies = BuildDependencyGraph(rules);

      // Part 1: Sum middle numbers of valid updates
      var part1 = updates
          .Where(update => IsValidOrder(update, dependencies))
          .Select(update => update[update.Count / 2])
          .Sum();

      // Part 2: Fix invalid updates and sum their middle numbers
      var part2 = updates
          .Where(update => !IsValidOrder(update, dependencies))
          .Select(update => GetCorrectOrder(update, dependencies))
          .Select(update => update[update.Count / 2])
          .Sum();

      return (part1, part2);
    }

    public (long, long, long, long) SolveSample()
    {
      var sample =
          @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";

      var (part1, part2) = Solve(sample);
      return (part1, part2, 143, 123);
    }
  }
}
