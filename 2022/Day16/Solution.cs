using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;

namespace AdventOfCode.Y2022.Day16;

[ProblemName("Proboscidea Volcanium")]
internal class Solution : ISolver
{
    private readonly Dictionary<(uint, uint), int> _pathLengths = new();
    private Graph _graph;
    private uint _startValve;

    private IReadOnlyDictionary<uint, Valve> _valves;
    private uint[] _valvesWithFlow;

    public object PartOne(string input)
    {
        ParseInput(input);

        var highScore = 0;
        Search(_startValve, _valvesWithFlow, (30, 0), false, ref highScore);

        return highScore;
    }

    public object PartTwo(string input)
    {
        ParseInput(input);

        var highScore = 0;
        Search(_startValve, _valvesWithFlow, (26, 0), true, ref highScore);

        return highScore;
    }

    private void Search(uint from, uint[] too, (int timeLeft, int Score) previous, bool elephant, ref int highScore)
    {
        (int timeLeft, int Score) Move(uint a, uint b, (int timeLeft, int Score) prev)
        {
            if (!_pathLengths.TryGetValue((a, b), out var dist))
            {
                dist = _graph.Dijkstra(a, b).Distance;
                _pathLengths.Add((a, b), dist);
            }

            var tEnd = prev.timeLeft - dist - 1;
            return (tEnd, prev.Score + tEnd * _valves[b].Flow);
        }

        foreach (var to in too)
        {
            var move = Move(from, to, previous);
            if (move.timeLeft >= 0)
            {
                if (move.Score > highScore) highScore = move.Score;

                if (too.Length > 1)
                    Search(to, too.Where(j => j != to).ToArray(), move, elephant,
                        ref highScore);
            }
            else if (elephant && previous.Score >= highScore / 2)
            {
                Search(_startValve, too, (26, previous.Score), false, ref highScore);
            }
        }
    }

    private void ParseInput(string input)
    {
        _graph = new Graph();
        var parsed = input.ReadLinesToType<string>()
            .Select(line =>
                Regex.Match(line, @"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]+)"))
            .Select(match => (Node: _graph.AddNode(), Name: match.Groups[1].Value,
                Flow: int.Parse(match.Groups[2].Value),
                Tunnels: match.Groups[3].Value.Split(", ")))
            .ToDictionary(valve => valve.Name, valve => valve);

        _valves = parsed.Values
            .Select(valve => new Valve(valve.Node, valve.Name, valve.Flow,
                valve.Tunnels.Select(s => parsed[s].Node).ToList()))
            .ToDictionary(valve => valve.Node, valve => valve);

        foreach (var (from, _, _, tunnels) in _valves.Values)
        foreach (var to in tunnels)
            _graph.Connect(from, to, 1);

        _startValve = _valves.Values.Single(valve => valve.Name == "AA").Node;
        _valvesWithFlow = _valves.Values.Where(valve => valve.Flow > 0).Select(valve => valve.Node).ToArray();
    }
}

internal record Valve(uint Node, string Name, int Flow, List<uint> Tunnels);