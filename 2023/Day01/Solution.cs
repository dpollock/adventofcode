using System;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var r = new Regex("[0-9]");
        var lines = input.ReadLinesToType<string>();
        var sum = 0;
        foreach (var line in lines)
        {
            var matches = r.Matches(line);
            var num1 = ConvertToNumber(matches.First().Value);
            var num2 = ConvertToNumber(matches.Last().Value);
            sum += num1 * 10 + num2;
        }

        return sum;
    }

    public object PartTwo(string input)
    {
        var r = new Regex("(?=(1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine))");
        var lines = input.ReadLinesToType<string>();
        var sum = 0;
        foreach (var line in lines)
        {
            var matches = r.Matches(line);
            var num1 = ConvertToNumber(matches.First().Groups[1].Value);
            var num2 = ConvertToNumber(matches.Last().Groups[1].Value);
            sum += num1 * 10 + num2;
        }

        return sum;
    }

    private int ConvertToNumber(string value)
    {
        switch (value)
        {
            case "one":
                return 1;
            case "two":
                return 2;
            case "three":
                return 3;
            case "four":
                return 4;
            case "five":
                return 5;
            case "six":
                return 6;
            case "seven":
                return 7;
            case "eight":
                return 8;
            case "nine":
                return 9;
            default:
                return Convert.ToInt32(value);
        }
    }
}