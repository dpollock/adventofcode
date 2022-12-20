using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day17;

[ProblemName("Pyroclastic Flow")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var result = new Tunnel(input, 100).AddRocks(2022).Height;
        return result;
    }

    public object PartTwo(string input)
    {
        var result = new Tunnel(input, 100).AddRocks(1000000000000).Height;
        return result;
    }

    private static IEnumerable<Pos> Area(string[] mat)
    {
        return from row in Enumerable.Range(0, mat.Length)
            from col in Enumerable.Range(0, mat[0].Length)
            select new Pos(row, col);
    }

    private static char Get(IEnumerable<IEnumerable<char>> mat, Pos pos)
    {
        return (mat.ElementAtOrDefault(pos.Row) ?? "#########").ElementAt(pos.Col);
    }

    private static void Set(IList<char[]> mat, Pos pos, char ch)
    {
        mat[pos.Row][pos.Col] = ch;
    }

    private class Tunnel
    {
        private readonly string _jets;

        private readonly List<char[]> _lines = new();
        private readonly int _linesToStore;

        private readonly string[][] _rocks;
        private ModCounter _iJet;
        private ModCounter _iRock;
        private long _linesNotStored;

        // Simulation runs so that only the top N lines are kept in the tunnel. 
        // This is a practical constant, there is NO THEORY BEHIND it.
        public Tunnel(string jets, int linesToStore)
        {
            _linesToStore = linesToStore;
            _rocks = new[]
            {
                new[] { "####" },
                new[] { " # ", "###", " # " },
                new[] { "  #", "  #", "###" },
                new[] { "#", "#", "#", "#" },
                new[] { "##", "##" }
            };
            _iRock = new ModCounter(0, _rocks.Length);

            _jets = jets;
            _iJet = new ModCounter(0, jets.Length);
        }

        public long Height => _lines.Count + _linesNotStored;

        public Tunnel AddRocks(long rocksToAdd)
        {
            // We are adding rocks one by one until we find a recurring pattern.

            // Then we can jump forward full periods with just increasing the height 
            // of the cave: the top of the cave should look the same after a full period
            // so no need to simulate he rocks anymore. 

            // Then we just add the remaining rocks. 

            var seen = new Dictionary<string, (long rocksToAdd, long height)>();
            while (rocksToAdd > 0)
            {
                var hash = string.Join("", _lines.SelectMany(ch => ch));
                if (seen.TryGetValue(hash, out var cache))
                {
                    // we have seen this pattern, advance forward as much as possible
                    var heightOfPeriod = Height - cache.height;
                    var periodLength = cache.rocksToAdd - rocksToAdd;
                    _linesNotStored += rocksToAdd / periodLength * heightOfPeriod;
                    rocksToAdd = rocksToAdd % periodLength;
                    break;
                }

                seen[hash] = (rocksToAdd, Height);
                AddRock();
                rocksToAdd--;
            }

            while (rocksToAdd > 0)
            {
                AddRock();
                rocksToAdd--;
            }

            return this;
        }

        // Adds one rock to the cave
        private void AddRock()
        {
            var rock = _rocks[(int)_iRock++];

            // make room of 3 lines + the height of the rock
            for (var i = 0; i < rock.Length + 3; i++) _lines.Insert(0, "|       |".ToArray());

            // simulate falling
            var pos = new Pos(0, 3);
            while (true)
            {
                var jet = _jets[(int)_iJet++];
                if (jet == '>' && !Hit(rock, pos.Right))
                    pos = pos.Right;
                else if (jet == '<' && !Hit(rock, pos.Left)) pos = pos.Left;
                if (Hit(rock, pos.Below)) break;
                pos = pos.Below;
            }

            Draw(rock, pos);
        }

        // tells if a rock can be placed in the given location or hits something
        private bool Hit(string[] rock, Pos pos)
        {
            return Area(rock).Any(pt =>
                Get(rock, pt) == '#' &&
                Get(_lines, pt + pos) != ' '
            );
        }

        private void Draw(string[] rock, Pos pos)
        {
            // draws a rock pattern into the cave at the given x,y coordinates,
            foreach (var pt in Area(rock))
                if (Get(rock, pt) == '#')
                    Set(_lines, pt + pos, '#');

            // remove empty lines from the top
            while (!_lines[0].Contains('#')) _lines.RemoveAt(0);

            // keep the tail
            while (_lines.Count > _linesToStore)
            {
                _lines.RemoveAt(_lines.Count - 1);
                _linesNotStored++;
            }
        }
    }

    private readonly record struct Pos(int Row, int Col)
    {
        public Pos Left => this with { Col = Col - 1 }; 
        public Pos Right => this with { Col = Col + 1 };
        public Pos Below => this with { Row = Row + 1 };

        public static Pos operator +(Pos posA, Pos posB)
        {
            return new Pos(posA.Row + posB.Row, posA.Col + posB.Col);
        }
    }

    private record struct ModCounter(int Index, int Mod)
    {
        public static explicit operator int(ModCounter c)
        {
            return c.Index;
        }

        public static ModCounter operator ++(ModCounter c)
        {
            return c with { Index = c.Index == c.Mod - 1 ? 0 : c.Index + 1 };
        }
    }
}