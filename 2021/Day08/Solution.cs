using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Y2021.Day08
{
    [ProblemName("Seven Segment Search")]
    internal class Solution : ISolver
    {
        public object PartOne(string input)
        {
            var lines = input.ReadLinesToType<string>();
            var entries = new List<Entry>();
            foreach (var line in lines)
            {
                var split = line.Split("|");
                entries.Add(new Entry
                {
                    Patterns = split[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList(),
                    Outputs = split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()
                });
            }


            var ones = entries.Sum(x => x.Outputs.Count(x => x.Length == 2));
            var fours = entries.Sum(x => x.Outputs.Count(x => x.Length == 2));
            var sevens = entries.Sum(x => x.Outputs.Count(x => x.Length == 3));
            var eights = entries.Sum(x => x.Outputs.Count(x => x.Length == 2));


            return ones + fours + sevens + eights;
        }

        public object PartTwo(string input)
        {
            long sum = 0;
            var lines = input.ReadLinesToType<string>();
            foreach (var line in lines)
            {
                Dictionary<int, List<char>> mapping = new();
                var tmp = line.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<List<char>> wires = new();
                foreach (var x in tmp[..10])
                {
                    var c = x.ToList();
                    c.Sort();
                    wires.Add(c);
                }

                List<string> outputs = new();

                foreach (var x in tmp[10..])
                {
                    var c = x.ToList();
                    c.Sort();
                    outputs.Add(new(c.ToArray()));
                }

                mapping[1] = wires.First(x => x.Count == 2);
                mapping[4] = wires.First(x => x.Count == 4);
                mapping[7] = wires.First(x => x.Count == 3);
                mapping[8] = wires.First(x => x.Count == 7);


                //Find 6, which is then used to find C and F
                mapping[6] = wires.First(x => x.Count == 6 && !mapping[7].All(a => x.Contains(a)) && !mapping.ContainsValue(x));
                char _c = mapping[1].Except(mapping[6]).First();
                char _f = mapping[1].Except(new char[] { _c }).First();

                //Use C and F to find 3, 2, and 5
                mapping[3] = wires.First(x => x.Count == 5 && (x.Contains(_c) && x.Contains(_f)) && !mapping.ContainsValue(x));
                mapping[2] = wires.First(x => x.Count == 5 && (x.Contains(_c) && !x.Contains(_f)) && !mapping.ContainsValue(x));
                mapping[5] = wires.First(x => x.Count == 5 && (!x.Contains(_c) && x.Contains(_f)) && !mapping.ContainsValue(x));

                //Use 5 and 6 to find E
                char _e = mapping[6].Except(mapping[5]).First();

                //Use E to find 0
                mapping[0] = wires.First(x => x.Count == 6 && x.Contains(_e) && !mapping.ContainsValue(x));

                //9 is the only one remaining
                mapping[9] = wires.First(x => !mapping.ContainsValue(x));

                var reverseMapping = mapping.ToDictionary(x => new string(x.Value.ToArray()), x => x.Key);
                StringBuilder sb = new();
                foreach (var o in outputs) sb.Append(reverseMapping[o]);
                sum += int.Parse(sb.ToString());
            }

            return sum;
        }



        private class Entry
        {
            public List<string> Patterns { get; set; } //10 
            public List<string> Outputs { get; set; } //4
        }
    }
}