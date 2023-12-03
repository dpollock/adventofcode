using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day03
{
    [ProblemName("Gear Ratios")]
    class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var numbers = new List<(int, int, int number)>();
            var symbols = new List<(int, int, char symbol)>();

            foreach (var (line, index) in lines.Select((line, index) => (line, index)))
            {
                numbers.AddRange(ParseLineForNumbers(line, index));
                symbols.AddRange(ParseLineForSymbols(line, index));
            }

            var result = symbols.SelectMany(s => PartsTouchedBySymbol(s, numbers)).Distinct().ToList();
            var sum = result.Sum(x => x.number);
            return sum;
        }

        public object PartTwo(string input)
        {
            var lines = input.ReadLinesToType<string>().ToList();

            var numbers = new List<(int, int, int number)>();
            var symbols = new List<(int, int, char symbol)>();

            foreach (var (line, index) in lines.Select((line, index) => (line, index)))
            {
                numbers.AddRange(ParseLineForNumbers(line, index));
                symbols.AddRange(ParseLineForSymbols(line, index));
            }

            var result = symbols.Where(s => s.symbol == '*')
                .GroupBy(s=>s)
                .Select(grp=>new {Sym = grp.Key, Parts = PartsTouchedBySymbol(grp.Key, numbers).ToList() })
                .Where(g=> g.Parts.Count == 2)
                .ToList();

            return result.Sum(x => x.Parts[0].number * x.Parts[1].number);
        }

        public static IEnumerable<(int x, int y)> GetNeighborsOfString(
            (int x, int y) p, int stringLength,
            int maxX, int maxY)
        {
            var result = new List<(int x, int y)>();

            for (int i = 0; i < stringLength; i++)
            {
                result.AddRange((p.x, p.y + i).GetCartesianNeighbors(true)
                    .Where(q =>
                        q.y >= 0 && q.y <= maxY
                                 && q.x >= 0 && q.x <= maxX));
            }

            return result.DistinctBy(tuple => tuple);
        }

        private IEnumerable<(int row, int col, int number)> PartsTouchedBySymbol((int row, int col, char symbol) symbol,
            List<(int row, int col, int number)> parts)
        {
            foreach (var part in parts)
            {
                var partNeighbors = GetNeighborsOfString((part.row, part.col), part.number.ToString().Length, 140, 140);
                foreach (var (i, j) in partNeighbors)
                {
                    if (symbol.row == i && symbol.col == j)
                        yield return part;
                }
            }
        }

        private static IEnumerable<(int row, int col, int number)> ParseLineForNumbers(string line, int lineNum)
        {
            var curNum = string.Empty;
            var curIndex = -1;
            foreach (var (c, index) in line.Select((c, index) => (c, index)))
            {
                if (char.IsDigit(c))
                {
                    if (curIndex == -1)
                    {
                        curIndex = index;
                    }

                    curNum += c;
                }
                else
                {
                    if (string.IsNullOrEmpty(curNum)) continue;
                    yield return (lineNum, curIndex, int.Parse(curNum));
                    curNum = string.Empty;
                    curIndex = -1;
                }
            }

            if (curIndex != -1)
            {
                yield return (lineNum, curIndex, int.Parse(curNum));
            }
        }

        private IEnumerable<(int row, int col, char symbol)> ParseLineForSymbols(string line, int lineNum)
        {
            foreach (var (c, index) in line.Select((c, index) => (c, index)))
            {
                if (!char.IsDigit(c) && c != '.')
                    yield return (lineNum, index, c);
            }
        }
    }
}