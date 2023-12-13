using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2023.Day13;

[ProblemName("Point of Incidence")]
internal class Solution : ISolver
{
    public (long, long) Solve(string input)
    {
        var mirrors = input.ReadLinesToGroupsOfType<string>().ToList();

        var part1 = 0;
        var part2 = 0;
        foreach (var line in mirrors)
        {
            //loop through the columns then the rows
            foreach (var (numColsOrRows, score, rowOrColSelector) in
                     new (int, int, Func<int, IEnumerable<char>>)[]
                     {
                         (line.Count, 100, it => line[it]),  //columns - number of line, score is 100, get lines by index
                         (line[0].Length, 1, it => line.Select(row => row[it])), //rows - number of chars in first line, score is 1, get cols by index
                     })
            {
                for (var i = 1; i < numColsOrRows; i++)
                {
                    var smudges = 0;
                    //starts with 1,0 then 2,1 then 3,2 etc. Comparing j and k. If they are different then that's a "smudge".
                    //Part 1 wants exact reflections, so no smudges. Part 2 wants reflections with one "smudge".
                    for (var (j, k) = (i, i - 1); j < numColsOrRows && k >= 0; (j, k) = (j + 1, k - 1))
                    {
                        smudges += rowOrColSelector(j).Zip(rowOrColSelector(k)).Count(it => it.First != it.Second);
                    }

                    part1 += smudges == 0 ? score * i : 0;
                    part2 += smudges == 1 ? score * i : 0;
                }
            }
        }

        return (part1, part2);
    }

   
}