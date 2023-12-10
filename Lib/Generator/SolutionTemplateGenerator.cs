using AdventOfCode.Model;

namespace AdventOfCode.Generator;

internal class SolutionTemplateGenerator
{
    public string Generate(Problem problem)
    {
        return $@"using System;
             |using System.Collections.Generic;
             |using System.Collections.Immutable;
             |using System.Linq;
             |using System.Text.RegularExpressions;
             |using System.Text;
             |using adventofcode.Lib;
             |
             |namespace AdventOfCode.Y{problem.Year}.Day{problem.Day.ToString("00")}
             |{{
             |  [ProblemName(""{problem.Title}"")]      
             |  class Solution : ISolver {{
             |
             |        public object PartOne(string input) {{
             |            var lines = input.ReadLinesToType<string>().ToList();
             |            return 0;
             |        }}
             |
             |        public object PartTwo(string input) {{
             |            var lines = input.ReadLinesToType<string>().ToList();
             |            return 0;
             |     }}
             |  }}
             |}}
             |".StripMargin();
    }
}