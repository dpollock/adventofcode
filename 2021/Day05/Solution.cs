using adventofcode.Lib;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day05
{
    [ProblemName("Hydrothermal Venture")]
    class Solution : ISolver
    {

        public object PartOne(string input)
        {
            var rows = input.ReadLinesToType<string>();

            var segments = new List<LineSegment>();

            var covered = new List<Point>();

            Regex r = new Regex(@"([0-9]+),([0-9]+)\s->\s([0-9]+),([0-9]+)");
            foreach (var row in rows)
            {
                var match = r.Match(row);
                var s = (new LineSegment()
                {
                    From = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    To = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });

                segments.Add(s);

                //Map it out
                if (s.From.X == s.To.X)
                {
                    var start = s.From.Y;
                    var finish = s.To.Y;
                    if (start > finish)
                    {
                        finish = s.From.Y;
                        start = s.To.Y;
                    }
                    for (int i = start; i <= finish; i++)
                    {
                        covered.Add(new Point(s.To.X, i));
                    }
                }
                else if (s.From.Y == s.To.Y)
                {
                    var start = s.From.X;
                    var finish = s.To.X;
                    if (start > finish)
                    {
                        finish = s.From.X;
                        start = s.To.X;
                    }
                    for (int i = start; i <= finish; i++)
                    {
                        covered.Add(new Point(i, s.To.Y));
                    }
                }
                
            }

            var uniqueSegments = covered.GroupBy(m => new { m.X, m.Y }).Select(x => new { x.Key, count = x.Count() }).ToList();
            var result = uniqueSegments.Count(x => x.count > 1);
            return result;
        }

        public object PartTwo(string input)
        {
            var rows = input.ReadLinesToType<string>();

            var segments = new List<LineSegment>();

            var covered = new List<Point>();

            Regex r = new Regex(@"([0-9]+),([0-9]+)\s->\s([0-9]+),([0-9]+)");
            foreach (var row in rows)
            {
                var match = r.Match(row);
                var s = (new LineSegment()
                {
                    From = new Point(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                    To = new Point(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))
                });

                segments.Add(s);

                var dx = Math.Sign(s.To.X - s.From.X);
                var dy = Math.Sign(s.To.Y - s.From.Y);

                for (int x = 0; x != s.To.X + dx ; x+= dx)
                {
                    for (int y = 0; y != s.To.Y + dy; y += dy)
                    {
                        covered.Add(new Point(x,y));
                    }
                }
                ////Map it out
                //if (s.From.X == s.To.X)
                //{
                //    var start = s.From.Y;
                //    var finish = s.To.Y;
                //    if (start > finish)
                //    {
                //        finish = s.From.Y;
                //        start = s.To.Y;
                //    }
                //    for (int i = start; i <= finish; i++)
                //    {
                //        covered.Add(new Point(s.To.X, i));
                //    }
                //}
                //else if (s.From.Y == s.To.Y)
                //{
                //    var start = s.From.X;
                //    var finish = s.To.X;
                //    if (start > finish)
                //    {
                //        finish = s.From.X;
                //        start = s.To.X;
                //    }
                //    for (int i = start; i <= finish; i++)
                //    {
                //        covered.Add(new Point(i, s.To.Y));
                //    }
                //}
                //else
                //{
                //    bool increaseX = s.From.X < s.To.X;
                //    bool increaseY = s.From.Y < s.To.Y;


                //    if (increaseX && increaseY)
                //    {
                //        for (int x = s.From.X; x <= s.To.X; x++)
                //        {
                //            for (int y = s.From.Y; y <= s.To.Y; y++)
                //            {
                //                covered.Add(new Point(x, y));
                //            }
                //        }
                //    }


                //    if (increaseX && !increaseY)
                //    {
                //        for (int x = s.From.X; x <= s.To.X; x++)
                //        {
                //            for (int y = s.From.Y; y >= s.To.Y; y--)
                //            {
                //                covered.Add(new Point(x, y));
                //            }
                //        }
                //    }

                //    if (!increaseX && increaseY)
                //    {
                //        for (int x = s.From.X; x >= s.To.X; x--)
                //        {
                //            for (int y = s.From.Y; y <= s.To.Y; y++)
                //            {
                //                covered.Add(new Point(x, y));
                //            }
                //        }
                //    }

                //    if (!increaseX && !increaseY)
                //    {
                //        for (int x = s.From.X; x >= s.To.X; x--)
                //        {
                //            for (int y = s.From.Y; y >= s.To.Y; y--)
                //            {
                //                covered.Add(new Point(x, y));
                //            }
                //        }
                //    }

                }
            }

            var uniqueSegments = covered.GroupBy(m => new { m.X, m.Y }).Select(x => new { x.Key, count = x.Count() }).ToList();
            var result = uniqueSegments.Count(x => x.count > 1);
            return result;
        }

        class Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        class LineSegment
        {
            public Point From { get; set; }
            public Point To { get; set; }
        }
    }
}
