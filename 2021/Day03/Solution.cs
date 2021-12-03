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

            var bits = Enumerable.Range(0, numOfBits)
                                                        .Select(i => rows
                                                                .Select(x => x[i])
                                                                .GroupBy(x => x).Select(x => new { bit = x.Key.ToString(), num = x.Count() })
                                                                .OrderByDescending(x => x.num));

            var mostCommon = "";
            var leastCommon = "";
            foreach (var bit in bits)
            {
                mostCommon += bit.First().bit;
                leastCommon += bit.Last().bit;
            }

            var gammaRate2 = Convert.ToInt32(mostCommon, 2);
            var epsilonRate2 = Convert.ToInt32(leastCommon, 2);

            return gammaRate2 * epsilonRate2;
        }

        public object PartTwo(string input)
        {
            var rows = input.ReadLinesToType<string>().ToList();
            var numOfBits = rows.First().Length;

            var bits = Enumerable.Range(0, numOfBits)
                .Select(i => rows
                    .Select(x => x[i])
                    .GroupBy(x => x).Select(x => new { bit = x.Key.ToString(), num = x.Count() })
                    .OrderByDescending(x => x.num));

            var mostCommon = "";
            var leastCommon = "";
            foreach (var bit in bits)
            {
                var most = bit.First();
                var least = bit.Last();
                mostCommon += most.bit;
                leastCommon += least.bit;
            }


            var oxygenCandidates = rows.Where(x => x[0] == mostCommon[0]).ToList();
            for (int i = 0; i < numOfBits && oxygenCandidates.Count() != 1; i++)
            {
                int onesCount = oxygenCandidates.Count(c => c[i] == '1');
                int zeroesCount = oxygenCandidates.Count() - onesCount;

                if (onesCount >= zeroesCount)
                {
                    oxygenCandidates = oxygenCandidates.Where(x => x[i] == '1').ToList();
                }
                else
                {
                    oxygenCandidates = oxygenCandidates.Where(x => x[i] == '0').ToList();
                }

            }

            var co2Candidates = rows.ToList();
            for (int i = 0; i < numOfBits && co2Candidates.Count() != 1; i++)
            {
                int onesCount = co2Candidates.Count(c => c[i] == '1');
                int zeroesCount = co2Candidates.Count() - onesCount;

                if (onesCount >= zeroesCount)
                {
                    co2Candidates = co2Candidates.Where(x => x[i] == '0').ToList();
                }
                else
                {
                    co2Candidates = co2Candidates.Where(x => x[i] == '1').ToList();
                }

            }

            var o2 = Convert.ToInt32(oxygenCandidates.FirstOrDefault() ?? "0", 2);
            var co2 = Convert.ToInt32(co2Candidates.FirstOrDefault() ?? "0", 2);

            return o2 * co2;
        }


    }
}
