using System;
using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var (pipes, start, size) = ParseInput(input);
        var (_, maxDistance) = BuildGraph(pipes, start, size);
        return maxDistance;
    }

    public object PartTwo(string input)
    {
        var (pipes, start, size) = ParseInput(input);
        var (graph, _) = BuildGraph(pipes, start, size);
        var insideLoop = CountPointsInsideLoop(start, graph, size);
        return insideLoop;
    }

    private static (Dictionary<(int x0, int y0), List<(int x1, int y1)>> graph, int maxDistance) BuildGraph(
        Dictionary<(int x0, int y0), ((int x1, int y1) connection1, (int x2, int y2) connection2)> pipes,
        (int x, int y) start, int size)
    {
        var graph = new Dictionary<(int x0, int y0), List<(int x1, int y1)>>();
        foreach (var pipe in pipes)
        {
            var (x, y) = pipe.Key;
            var ((dx1, dy1), (dx2, dy2)) = pipe.Value;
            if (!graph.ContainsKey((x, y))) graph[(x, y)] = new List<(int dx, int dy)>();

            AddConnection(pipe.Key, (dx1, dy1));
            AddConnection(pipe.Key, (dx2, dy2));
        }

        //Helper to check if the connection already exits
        void AddConnection((int x, int y) current, (int dx, int dy) connection)
        {
            if (pipes.TryGetValue(connection, out var connections) &&
                (connections.connection1 == current || connections.connection2 == current))
                graph[current].Add(connection);
        }

        // add the starting point
        graph[start] = new List<(int x, int y)>();

        foreach (var pipe in pipes)
            if (pipe.Key != start && (pipe.Value.connection1 == start || pipe.Value.connection2 == start))
                graph[start].Add(pipe.Key);

        var visited = new bool[size, size];
        var maxDistance = -1;

        visited[start.x, start.y] = true;

        //using a queue to walk the route because using recursion would cause a stack overflow
        var queue = new Queue<((int x, int y) p, int n, List<(int x, int y)> path)>();
        queue.Enqueue((start, 0, new List<(int x, int y)>()));
        while (queue.Count > 0)
        {
            var work = queue.Dequeue();
            foreach (var connection in graph[work.p])
                ProcessPipe(connection, work.n, work.path);
        }

        void ProcessPipe((int x, int y) point, int n, List<(int x, int y)> path)
        {
            if (!graph.ContainsKey(point) || visited[point.x, point.y]) return;

            n += 1;
            path.Add(point);
            visited[point.x, point.y] = true;
            maxDistance = Math.Max(maxDistance, n);
            queue.Enqueue((point, n, path));
        }

        return (graph, maxDistance);
    }

    private static int CountPointsInsideLoop((int x, int y) start,
        Dictionary<(int x0, int y0), List<(int x1, int y1)>> graph, int size)
    {
        // Code to process part 2 of the problem. It's not needed for part 1.
        var loop = new List<(int x, int y)>();
        var current = start;
        var prev = (-1, -1);
        do
        {
            loop.Add(current);
            // Find the next point in the loop that is not the previous point
            var next = graph[current].FirstOrDefault(p => p != prev);
            prev = current;
            current = next;
        } while (current != start && current != (0, 0)); // we're assuming (0, 0) won't be part of the loop

        // go through the whole map and check if the point is inside the loop
        var insideLoop = 0;
        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                if (IsPointInsidePolygon(loop, (i, j)))
                    insideLoop++;
        return insideLoop;
    }

    private static (Dictionary<(int x0, int y0), ((int x1, int y1) connection1, (int x2, int y2) connection2)> pipes, (
        int x, int y) start, int size) ParseInput(string input)
    {
        var pipes = new Dictionary<(int x0, int y0), ((int x1, int y1) connection1, (int x2, int y2) connection2)>();
        var start = (x: -1, y: 0);
        var map = input.GetCharMap();

        for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                switch (map[y][x])
                {
                    case '.':
                        continue;
                    case 'S':
                        start = (x, y);
                        break;
                    default:
                        pipes.Add((x, y), GetPipeConnections(map[y][x], x, y));
                        break;
                }


        return (pipes, start, size: map[0].Length);
    }


    //use a 'ray casting' algorithm to check if a point is inside a polygon
    private static bool IsPointInsidePolygon(List<(int x, int y)> loopPoints, (int x, int y) point)
    {
        if (loopPoints.Contains(point)) return false;

        var intersections = 0;
        var n = loopPoints.Count;

        for (var i = 0; i < n; i++)
        {
            var (x1, y1) = loopPoints[i];
            var (x2, y2) = loopPoints[(i + 1) % n];
            if (y1 > point.y != y2 > point.y)
            {
                var intersectX = x1 + (point.y - y1) * (x2 - x1) / (y2 - y1);

                if (intersectX > point.x) intersections++;
            }
        }

        // When the count is odd, we are starting from inside the loop (crossed to the other side and not back in)
        return intersections % 2 != 0;
    }

    private static ((int dx1, int dy1), (int dx2, int dy2)) GetPipeConnections(char pipe, int x, int y)
    {
        return pipe switch
        {
            '|' => ((x, y - 1), (x, y + 1)),
            '-' => ((x - 1, y), (x + 1, y)),
            'L' => ((x, y - 1), (x + 1, y)),
            'J' => ((x, y - 1), (x - 1, y)),
            '7' => ((x - 1, y), (x, y + 1)),
            'F' => ((x + 1, y), (x, y + 1)),
            _ => throw new Exception($"What is this? {pipe}")
        };
    }
}