using System.Linq;
using adventofcode.Lib;
using AngleSharp.Text;
using MoreLinq;

namespace AdventOfCode.Y2022.Day03;

[ProblemName("Rucksack Reorganization")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var sacks = input.ReadLinesToObject<Sack>();
        var result = sacks.Sum(s => s.Priority);
        return result;
    }

    public object PartTwo(string input)
    {
        var sacks = input.ReadLinesToObject<Sack>();
        var groupedSacks = sacks.Batch(3);
        var commonCharsInGroup = groupedSacks.Select(gs =>
            gs.ToList()[0].FullSack.Intersect(gs.ToList()[1].FullSack.Intersect(gs.ToList()[2].FullSack)).First());

        var result = commonCharsInGroup.Sum(s => s.IsUppercaseAscii() ? s - 'A' + 1 + 26 : s - 'a' + 1);
        return result;
    }
}

internal class Sack
{
    public string FullSack { get; set; }
    public string Compartment1 => FullSack.Substring(0, FullSack.Length / 2);

    public string Compartment2 => FullSack.Substring(FullSack.Length / 2);

    public char InBoth => Compartment1.Intersect(Compartment2).First();

    public int Priority => InBoth.IsUppercaseAscii() ? InBoth - 'A' + 1 + 26 : InBoth - 'a' + 1;
}