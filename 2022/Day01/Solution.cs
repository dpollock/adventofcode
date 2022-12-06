using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day01
{
  [ProblemName("Calorie Counting")]      
  class Solution : ISolver {

        public object PartOne(string input) {
            
            var lines = input.ReadLinesToGroupsOfType<int>();

            //elf with the most calories
            var result = lines.Max(elf => elf.Sum());

            return result;
        }

        public object PartTwo(string input) {

            var lines = input.ReadLinesToGroupsOfType<int>();

            //top 3 elfs with the most calories
            var result = lines.OrderByDescending(elf => elf.Sum()).Take(3).Sum(elf => elf.Sum());
            return result;
     }
  }
}
