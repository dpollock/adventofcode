using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day15;

[ProblemName("Lens Library")]
internal partial class Solution : ISolver
{
    public (long, long) Solve(string input)
    {
        var lines = input.ReadLinesToType<string>().ToList();
        var part1 = lines.Select(l => l.Split(',')).SelectMany(l => l).Select(HASH).Sum();
        var part2 = lines.Select(l => l.Split(',').Select(ParseStep)).Select(HASHMAP).Sum();

        return (part1, part2);
    }

    public (long, long, long, long) SolveSample()
    {
        var sample =
            """
            rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
            """;
        var lines = sample.ReadLinesToType<string>().ToList();
        var part1 = lines.Select(l => l.Split(',')).SelectMany(l => l).Select(HASH).Sum();
        var part2 = lines.Select(l => l.Split(',').Select(ParseStep)).Select(HASHMAP).Sum();
        ;

        return (part1, part2, 1320, 145);
    }


    private static int HASH(string s)
    {
        return s.Aggregate(0, (h, c) => (h + c) * 17 % 256);
    }


    private static int HASHMAP(IEnumerable<(string label, bool add, int? focalLength)> steps)
    {
        var boxes = new List<(string label, int focalLength)>[256];
        for (var i = 0; i < 256; i++) boxes[i] = new List<(string label, int focalLength)>();

        foreach (var (label, add, focalLength) in steps)
        {
            var hash = HASH(label);
            var box = boxes[hash];
            var pos = box.FindIndex(b => b.label == label);

            if (add)
            {
                var newStep = (label, focalLength ?? 0);
                if (pos == -1) box.Add(newStep);
                else box[pos] = newStep;
            }
            else if (pos != -1)
            {
                box.RemoveAt(pos);
            }
        }

        return boxes.SelectMany((box, i) => box.Select((step, j) => (i + 1) * (j + 1) * step.focalLength)).Sum();
    }

    private static (string label, bool add, int? focalLength) ParseStep(string step)
    {
        var groups = StepRegex().Match(step).Groups;
        return (groups["label"].Value, groups["add"].Value == "=", string.IsNullOrEmpty(groups["focalLength"].Value) ? null : int.Parse(groups["focalLength"].Value));
    }

    [GeneratedRegex(@"(?<label>.+)(?<add>[=-])(?<focalLength>\d+)?")]
    private static partial Regex StepRegex();

}