using System.Linq;
using MoreLinq;

namespace AdventOfCode.Y2022.Day06;

[ProblemName("Tuning Trouble")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var result = DistinctMarkWindow(input, 4);
        return result;
    }

    public object PartTwo(string input)
    {
        var result = DistinctMarkWindow(input, 14);
        return result;
    }

    private static object DistinctMarkWindow(string input, int windowSize)
    {
        var index = 0;
        foreach (var charGroup in input.Window(windowSize))
        {
            if (charGroup.Distinct().Count() == windowSize)
            {
                index += windowSize;
                break;
            }

            index++;
        }

        return index;
    }
}