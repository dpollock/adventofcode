using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var packets = input.ReadLinesToType<string>().Where(l => !string.IsNullOrWhiteSpace(l)).Chunk(2);
        var validPackets = packets.Select((pair, idx) => Compare(pair[0], pair[1]) == -1 ? idx + 1 : 0)
            .Where(i => i != 0).ToList();
        var result = validPackets.Sum();
        return result;
    }

    public object PartTwo(string input)
    {
        var packets = input.ReadLinesToType<string>().Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
        packets.Add("[[2]]");
        packets.Add("[[6]]");
        packets.Sort(Compare);
        var d1 = packets.IndexOf("[[2]]") + 1;
        var d2 = packets.IndexOf("[[6]]") + 1;
        return d1 * d2;
    }

    private string ChangeToArrayString(string s)
    {
        return s.All(char.IsDigit) ? $"[{s}]" : s;
    }

    private (string token, string rest) GetToken(string s)
    {
        if (char.IsDigit(s[0]))
        {
            var token = new string(s.TakeWhile(char.IsDigit).ToArray());
            var next = token.Length == s.Length ? token.Length : token.Length + 1;
            return (token, s[next..]);
        }

        var depth = 0;
        var pos = 0;
        while (!(depth == 0 && (pos == s.Length || s[pos] == ',')))
        {
            if (s[pos] == '[') depth++;
            if (s[pos] == ']') depth--;
            pos++;
        }

        return pos == s.Length ? (s, "") : (s[..pos], s[(pos + 1)..]);
    }

    private int Compare(string p1, string p2)
    {
        while (p1.Length > 0 && p2.Length > 0)
        {
            var t1 = GetToken(p1);
            var t2 = GetToken(p2);
            if (t1.token.Length == 0) return -1;
            if (t2.token.Length == 0) return 1;
            if (t1.token.All(char.IsDigit) && t2.token.All(char.IsDigit))
            {
                var n1 = int.Parse(t1.token);
                var n2 = int.Parse(t2.token);
                if (n1 < n2) return -1;
                if (n1 > n2) return 1;
            }
            else
            {
                if ((t1.token[0] == '[') ^ (t2.token[0] == '['))
                {
                    t1.token = ChangeToArrayString(t1.token);
                    t2.token = ChangeToArrayString(t2.token);
                }

                var compare = Compare(t1.token[1..^1], t2.token[1..^1]);
                if (compare != 0) return compare;
            }

            p1 = t1.rest;
            p2 = t2.rest;
        }

        return p2.Length > 0 ? -1 : p1.Length > 0 ? 1 : 0;
    }
}