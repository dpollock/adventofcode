using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day04;

[ProblemName("Camp Cleanup")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var pairs = input.ReadLinesToObject<PairRange>(",");

        var result = pairs.Count(x => x.DoRangesIncludeEachOther);
        return result;
    }

    public object PartTwo(string input)
    {
        var pairs = input.ReadLinesToObject<PairRange>(",");

        var result = pairs.Count(x => x.DoRangesOverLapAtAll);
        return result;
    }
}

internal class PairRange
{
    public string Range1 { get; set; }
    public string Range2 { get; set; }

    public List<int> Range1Numbers => ConvertToRange(Range1);
    public List<int> Range2Numbers => ConvertToRange(Range2);

    public bool DoRangesIncludeEachOther =>
        !Range1Numbers.Except(Range2Numbers).Any() || !Range2Numbers.Except(Range1Numbers).Any();

    public bool DoRangesOverLapAtAll =>
        Range1Numbers.Intersect(Range2Numbers).Any();

    private List<int> ConvertToRange(string range)
    {
        var parts = range.Split('-');

        var start = int.Parse(parts[0]);
        var end = int.Parse(parts[1]);

        var numbers = Enumerable.Range(start, end - start + 1).ToList();

        return numbers;
    }
}