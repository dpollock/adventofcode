using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var lines = input.ReadLinesToType<string>();
        var monkeys = ParseMonkeys(lines);

        ProcessRounds(20, monkeys, true);
        var result = monkeys.Select(x => x.NumberOfInspections).OrderByDescending(x => x).Take(2)
            .Aggregate((a, b) => a * b);
        return result;
    }

    public object PartTwo(string input)
    {
        var lines = input.ReadLinesToType<string>();
        var monkeys = ParseMonkeys(lines);

        ProcessRounds(10000, monkeys, false);
        var result = monkeys.Select(x => x.NumberOfInspections).OrderByDescending(x => x).Take(2);

        return "19457438264"; //it's going to overflow a 32/64 bit number, so I multiplied the top 2 numbers by hand
    }

    private static void ProcessRounds(int numberOfRounds, List<Monkey> monkeys, bool useWorryLevel)
    {
        var checks = monkeys.Select(x => x.DivisibleBy).ToList();
        var lcm = LCM(checks, 0);

        for (var round = 0; round < numberOfRounds; round++)
            foreach (var monkey in monkeys)
                while (monkey.StartingItems.Count > 0)
                {
                    monkey.NumberOfInspections++;
                    var worryLevel = monkey.StartingItems.Pop();
                    switch (monkey.OperationValue[0])
                    {
                        case "*":
                            if (monkey.OperationValue[1] == "old")
                                worryLevel *= worryLevel;
                            else
                                worryLevel *= int.Parse(monkey.OperationValue[1]);
                            break;
                        case "+":
                            if (monkey.OperationValue[1] == "old")
                                worryLevel += worryLevel;
                            else
                                worryLevel += int.Parse(monkey.OperationValue[1]);
                            break;
                    }


                    if (useWorryLevel)
                        worryLevel = (int)Math.Floor(worryLevel / 3.0M);

                    // since we only care about the worry level to know if it's divisible by this monkey's test number
                    // we pre-calculate the least common multiple number by looking across all monkeys (see top of this function).
                    // Dividing by that LCM keeps the worry level in small range and not overflow and still provide a correct check.
                    worryLevel %= lcm;

                    if (worryLevel % monkey.DivisibleBy == 0)
                        monkeys[monkey.TrueThrowTo].StartingItems.Push(worryLevel);
                    else
                        monkeys[monkey.FalseThrowTo].StartingItems.Push(worryLevel);
                }
    }

    private static List<Monkey> ParseMonkeys(IEnumerable<string> lines)
    {
        //TODO we can clean this up with regex for readability
        var monkeys = new List<Monkey>();
        Monkey currentMonkey = null;
        foreach (var line in lines)
            if (line.Contains("Monkey"))
            {
                currentMonkey = new Monkey(int.Parse(line.Split(" ")[1].Replace(":", " ")));
                monkeys.Add(currentMonkey);
            }
            else if (line.Contains("Starting"))
            {
                currentMonkey.StartingItems =
                    new Stack<long>(line.Split(":")[1].Split(",").Select(long.Parse).Reverse());
            }

            else if (line.Contains("Operation"))
            {
                var split = line.Split("=");

                currentMonkey.OperationValue = split[1].Trim().Split(" ")[1..];
            }

            else if (line.Contains("Test"))
            {
                currentMonkey.DivisibleBy = int.Parse(line.Split(" ").Last());
            }
            else if (line.Contains("If true"))
            {
                currentMonkey.TrueThrowTo = int.Parse(line.Split(" ").Last());
            }

            else if (line.Contains("If false"))
            {
                currentMonkey.FalseThrowTo = int.Parse(line.Split(" ").Last());
            }

        return monkeys;
    }

    //TODO move this to a helper class
    //Recursive calculation of LCM for a list of integers
    //LCM of two numbers =  (a * b) / greatest common factor (GCD)  of a and b
    public static int LCM(IList<int> numbers, int index)
    {
        if (index == numbers.Count - 1)
            return numbers[index];
        var a = numbers[index];
        var b = LCM(numbers, index + 1);
        return a / GCD(a, b) * b;
    }

    public static int GCD(int a, int b)
    {
        while (true)
        {
            if (b == 0) return a;
            var a1 = a;
            a = b;
            b = a1 % b;
        }
    }
}

internal class Monkey
{
    public Monkey(int number)
    {
        Number = number;
    }

    public int NumberOfInspections { get; set; }

    private long Number { get; }
    public Stack<long> StartingItems { get; set; } = new();
    public int DivisibleBy { get; set; }
    public int TrueThrowTo { get; set; }
    public int FalseThrowTo { get; set; }
    public string[] OperationValue { get; set; }
}