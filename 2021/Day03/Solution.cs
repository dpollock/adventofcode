using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day03
{
    [ProblemName("Binary Diagnostic")]
    class Solution : ISolver
    {

        public object PartOne(string input)
        {

            var rows = input.ReadLinesToType<string>().ToList();
            var numOfBits = rows.First().Length;

            var bits  = Enumerable.Range(0, numOfBits)
                                                        .Select(i => rows
                                                                .Select(x => x[i])
                                                                .GroupBy(x => x).Select(x => new { bit = x.Key.ToString(), num = x.Count() })
                                                                .OrderByDescending(x=>x.num));

            var gammaRate = "";
            var epsilonRate = "";
            foreach (var bit in bits)
            {
                gammaRate += bit.First().bit;
                epsilonRate += bit.Last().bit;
            }

            var gammaRate2 = Convert.ToInt32(gammaRate, 2);
            var epsilonRate2 = Convert.ToInt32(epsilonRate, 2);
            return gammaRate2 * epsilonRate2;
        }

        public object PartTwo(string input)
        {
            var rows = input.ReadLinesToType<string>();
            return 0;
        }
    }
}
