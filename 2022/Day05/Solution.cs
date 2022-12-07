using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day05;

[ProblemName("Supply Stacks")]
internal class Solution : ISolver
{
    private List<MoveCommand> _commands;
    private List<SupplyStack> _stacks;


    public object PartOne(string input)
    {
        PopulateStacksAndCommands(input);

        //play the commands
        foreach (var command in _commands)
            for (var num = 0; num < command.Number; num++)
            {
                var crate = _stacks[command.From - 1].Crates.Pop();
                _stacks[command.To - 1].Crates.Push(crate);
            }

        //peek the top of each stack to get answer
        var result = "";
        foreach (var stack in _stacks) result += stack.Crates.Peek();

        return result;
    }

    public object PartTwo(string input)
    {
        PopulateStacksAndCommands(input);

        //play the commands
        foreach (var command in _commands)
        {
            var cratesToMove = new List<string>();

            for (var num = 0; num < command.Number; num++) cratesToMove.Add(_stacks[command.From - 1].Crates.Pop());

            cratesToMove.Reverse();
            foreach (var crate in cratesToMove) _stacks[command.To - 1].Crates.Push(crate);
        }

        //peek the top of each stack to get answer
        var result = "";
        foreach (var stack in _stacks) result += stack.Crates.Peek();

        return result;
    }

    private void PopulateStacksAndCommands(string input)
    {
        _stacks = new List<SupplyStack>();
        _commands = new List<MoveCommand>();
        for (var i = 0; i < 9; i++) _stacks.Add(new SupplyStack());

        //parse starting position of crates, we reverse it since we're pushing it onto a stack.
        var startingPositions = input.ReadLinesToType<string>().Take(8);
        foreach (var row in startingPositions.Reverse())
            for (var i = 0; i < 9; i++)
            {
                var value = row.Substring(i * 4 + 1, 1);
                if (!string.IsNullOrWhiteSpace(value)) _stacks[i].Crates.Push(value);
            }

        //parse the raw commands with regex
        var rawCommand = input.ReadLinesToType<string>().Skip(10).ToList();
        foreach (var command in rawCommand)
        {
            var regex = new Regex(@"^move\s(?<number>\d*)\sfrom\s(?<from>\d*)\sto\s(?<to>\d*)$");
            var matches = regex.Matches(command);
            if (matches.Count > 0)
                _commands.Add(new MoveCommand(int.Parse(matches[0].Groups["number"].Value),
                    int.Parse(matches[0].Groups["from"].Value), int.Parse(matches[0].Groups["to"].Value)));
        }
    }
}

internal class SupplyStack
{
    public Stack<string> Crates { get; set; } = new();
}

internal readonly record struct MoveCommand(int Number, int From, int To);