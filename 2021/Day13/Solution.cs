using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode.Y2021.Day13
{
    [ProblemName("Transparent Origami")]
    class Solution : ISolver
    {

        public object PartOne(string input) => GetFolds(input).First().Count();

        //print the solution, upload by hand
        public object PartTwo(string input) => ToString(GetFolds(input).Last());

        IEnumerable<HashSet<Point>> GetFolds(string input)
        {
            var blocks = input.Split("\n\n");
            var points = (
                from line in blocks[0].Split("\n")
                let coords = line.Split(",")
                select new Point(int.Parse(coords[0]), int.Parse(coords[1]))
            ).ToHashSet();

            // fold line by line, yielding a new hashset
            foreach (var line in blocks[1].Split("\n"))
            {
                var rule = line.Split("=");
                if (rule[0].EndsWith("x"))
                {
                    points = FoldX(int.Parse(rule[1]), points);
                }
                else
                {
                    points = FoldY(int.Parse(rule[1]), points);
                }
                yield return points;
            }
        }

        string ToString(HashSet<Point> d)
        {
            var res = "\n\n\n\n";
            var height = d.OrderByDescending(p => p.y).First().y;
            var width = d.OrderByDescending(p => p.x).First().x;
            for (var y = 0; y <= height; y++)
            {
                for (var x = 0; x <= width; x++)
                {
                    res += d.Contains(new Point(x, y)) ? '#' : ' ';
                }
                res += "\n";
            }
            return res;
        }

        HashSet<Point> FoldX(int x, HashSet<Point> d) =>
            d.Select(p => p.x > x ? p with { x = 2 * x - p.x } : p).ToHashSet();

        HashSet<Point> FoldY(int y, HashSet<Point> d) =>
            d.Select(p => p.y > y ? p with { y = 2 * y - p.y } : p).ToHashSet();
    }
    record Point(int x, int y);

}
