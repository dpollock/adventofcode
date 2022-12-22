using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day21;

[ProblemName("Monkey Math")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var monkeys = ParseInput(input);

        long CalculateValue(string name)
        {
            var monkey = monkeys.First(x => x.Name == name);
            if (monkey.Value.HasValue)
                return monkey.Value.Value;

            var leftMonkeyValue = CalculateValue(monkey.LeftMonkey);
            var rightMonkeyValue = CalculateValue(monkey.RightMonkey);
            monkey.Value = monkey.Operation switch
            {
                "+" => leftMonkeyValue + rightMonkeyValue,
                "*" => leftMonkeyValue * rightMonkeyValue,
                "-" => leftMonkeyValue - rightMonkeyValue,
                "/" => leftMonkeyValue / rightMonkeyValue,
                _ => throw new NotImplementedException()
            };

            return monkey.Value.Value;
        }

        var result = CalculateValue("root");

        return result;
    }

    public object PartTwo(string input)
    {
        var monkeys = ParseInput(input);

        long? CalculateValue(string name)
        {
            var monkey = monkeys.First(x => x.Name == name);
            if (monkey.Name == "humn") return monkey.Value;

            if (monkey.Value.HasValue)
                return monkey.Value.Value;

            var leftMonkeyValue = CalculateValue(monkey.LeftMonkey);
            var rightMonkeyValue = CalculateValue(monkey.RightMonkey);
            if (leftMonkeyValue.HasValue && rightMonkeyValue.HasValue)
                monkey.Value = monkey.Operation switch
                {
                    "+" => leftMonkeyValue + rightMonkeyValue,
                    "*" => leftMonkeyValue * rightMonkeyValue,
                    "-" => leftMonkeyValue - rightMonkeyValue,
                    "/" => leftMonkeyValue / rightMonkeyValue,
                    _ => throw new NotImplementedException()
                };

            return monkey.Value;
        }

        var human = monkeys.First(x => x.Name == "humn");
        human.Value = null;
        var rootMonkey = monkeys.First(x => x.Name == "root");

        var right =CalculateValue(rootMonkey.RightMonkey);
        var left = (CalculateValue(rootMonkey.LeftMonkey) ?? 0);
        var missingValues = monkeys.Where(x => x.Value == null).ToList();

        long humanValue;
        long min = 0;
        long max = long.MaxValue;
        human.Value = 0;

        while (true)
        {
            if (left > right)
                min += (max - min) / 2;
            else
                max -= (max - min) / 2;
            
            human.Value = min + (max - min) / 2;
            humanValue = human.Value.Value;
            left = CalculateValue(rootMonkey.LeftMonkey).GetValueOrDefault();
            foreach (var monkey in missingValues) monkey.Value = null;

            if (left == right)
                break;
        }

        return humanValue;
    }

    private static HashSet<Monkey> ParseInput(string input)
    {
        var monkeys = new HashSet<Monkey>(new MonkeyComparer());
        var lines = input.ReadLinesToType<string>();
        foreach (var line in lines)
        {
            var monkey = new Monkey
            {
                Name = line.Split(':')[0].Trim(),
                Operation = line.Split(':')[1].Trim()
            };

            var op = line.Split(':')[1].Trim();
            if (int.TryParse(op, out var value))
            {
                monkey.Value = value;
            }
            else
            {
                var ops = op.Split(' ');
                monkey.LeftMonkey = ops[0];
                monkey.Operation = ops[1];
                monkey.RightMonkey = ops[2];
            }

            monkeys.Add(monkey);
        }

        return monkeys;
    }
}

internal class Monkey
{
    public string Name { get; set; }
    public string Operation { get; set; }
    public string LeftMonkey { get; set; }
    public string RightMonkey { get; set; }
    public long? Value { get; set; }
}

internal class MonkeyComparer : IEqualityComparer<Monkey>
{
    public bool Equals(Monkey x, Monkey y)
    {
        return x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode(Monkey obj)
    {
        return obj.Name.GetHashCode();
    }
}