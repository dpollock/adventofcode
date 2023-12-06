using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day04
{
    [ProblemName("Scratchcards")]
    partial class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var lines = input.ReadLinesToType<string>();
            var sum = 0;
            foreach (var line in lines)
            {
                var lineSplit = line.Split(":");

                var numberSplit = lineSplit[1].Split("|");

                var winningNumbers = NumberRegex().Matches(numberSplit[0]).Select(x => int.Parse(x.Value));
                var guessesNumbers = NumberRegex().Matches(numberSplit[1]).Select(x => int.Parse(x.Value));

                var matches = guessesNumbers.Join(winningNumbers, x => x, x => x, (l, r) => l);

                if (matches.Any())
                {
                    sum += (int)Math.Pow(2, matches.Count() - 1);
                }
            }


            return sum;
        }

        public object PartTwo(string input)
        {
            var lines = input.ReadLinesToType<string>();
            var instanceCounts = new List<int>();

            var index = 1;
            foreach (var line in lines)
            {
                var lineSplit = line.Split(":");

                var numberSplit = lineSplit[1].Split("|");

                var winningNumbers = NumberRegex().Matches(numberSplit[0]).Select(x => int.Parse(x.Value));
                var guessesNumbers = NumberRegex().Matches(numberSplit[1]).Select(x => int.Parse(x.Value));

                var matches = guessesNumbers.Intersect(winningNumbers);

                if (instanceCounts.Count < index)
                    instanceCounts.Add(1);
                else
                    instanceCounts[index - 1]++;

                if (matches.Any())
                {
                    var currentCardCopies = instanceCounts[index - 1];
                    foreach (var cardNumber in Enumerable.Range(index + 1, matches.Count()))
                    {
                        if (instanceCounts.Count < cardNumber)
                            instanceCounts.Add(currentCardCopies);
                        else
                            instanceCounts[cardNumber - 1] += currentCardCopies;
                    }
                }

                index++;
            }


            return instanceCounts.Sum();
        }

        [GeneratedRegex(@"\w+\s+(\d+)")]
        private static partial Regex CardRegex();

        [GeneratedRegex(@"(\d+)")]
        private static partial Regex NumberRegex();
    }
}