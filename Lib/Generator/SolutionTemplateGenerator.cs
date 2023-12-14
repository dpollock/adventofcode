using AdventOfCode.Model;

namespace AdventOfCode.Generator;

internal class SolutionTemplateGenerator
{
    public string Generate(Problem problem)
    {
        return $@"using System;
             |using System.Collections.Generic;
             |using System.Linq;
             |using adventofcode.Lib;
             |
             |namespace AdventOfCode.Y{problem.Year}.Day{problem.Day:00}
             |{{
             |  [ProblemName(""{problem.Title}"")]      
             |  class Solution : ISolver {{
             |
             |       public (long, long) Solve(string input)
             |        {{
             |            var lines = input.ReadLinesToType<string>();
             |       
             |            var part1 = 0L;
             |            var part2 = 0L;
             |            foreach (var line in lines)
             |            {{
             |               
             |            }}
             |       
             |       
             |            return (part1, part2);
             |        }}
             |  }}
             |}}
             |
             |
             |       public (long, long, long, long) SolveSample()
             |        {{
             |            var sample =
             |              """"""
             |              """""".ReadLinesToType<string>();
             |              var part1 = 0;
             |              var part2 = 0;
             |
             |              return (part1, part2, 0, 0);
             |        }}
             |  }}
             |}}
             |".StripMargin();
    }
}