using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day10
{
    [ProblemName("Syntax Scoring")]
    class Solution : ISolver
    {

        public object PartOne(string input)
        {
            var lines = ReadLines(input);


            var result = lines
                .Where(x => x.corrupted)
                .Sum(x => x.value);

            return result;
        }

        private static IEnumerable<(bool corrupted, long value)> ReadLines(string input)
        {
            var lines = input.ReadLinesToType<string>().Select(line =>
            {
                var stack = new Stack<char>();

                foreach (var currentChar in line.ToCharArray())
                {
                    if (currentChar is '{' or '(' or '[' or '<')
                    {
                        stack.Push(currentChar);
                    }
                    else
                    {
                        //character is a "closing" symbol
                        //pop the stack and compare to the current character
                        var v =
                            (stack.Pop(), c: currentChar) switch
                            {
                                // if (), then we're good
                                // if {), [), <), then bail (i.e. their mismatches
                                ('(', ')') => -1,
                                (_, ')') => 3,

                                ('[', ']') => -1,
                                (_, ']') => 57,

                                ('{', '}') => -1,
                                (_, '}') => 1197,

                                ('<', '>') => -1,
                                (_, '>') => 25137,
                            };

                        // corrupted? return the value
                        if (v != -1)
                            return (corrupted: true, value: v);
                    }
                }

                // not corrupted, calculate score
                var score = 0L;
                foreach (var c in stack)
                    score = score * 5 + c switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4,
                    };
                return (corrupted: false, value: score);
            });
            return lines;
        }

        public object PartTwo(string input)
        {
            var lines = ReadLines(input);

            var scores = lines
                .Where(x => !x.corrupted)
                .Select(x => x.value)
                .OrderBy(x => x)
                .ToList();

            return scores[scores.Count / 2].ToString();
        }
    }
}
