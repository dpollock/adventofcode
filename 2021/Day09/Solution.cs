using adventofcode.Lib;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day09
{
    [ProblemName("Smoke Basin")]
    class Solution : ISolver
    {
        int sizei = 100;
        int sizej = 100;
        int[,] grid = new int[100, 100];

        public object PartOne(string input)
        {
            
            var lines = input.ReadLinesToType<string>().ToList();

            for (int i = 0; i < sizei; i++)
            {
                for (int j = 0; j < sizej; j++)
                {
                    grid[i, j] = int.Parse(lines[i][j].ToString());
                }
            }

            var lowpoints = new List<int>();
            for (int i = 0; i < sizei; i++)
            {
                for (int j = 0; j < sizej; j++)
                {
                    var result = true;
                    if (i < sizei - 1)
                    {
                        result &= grid[i, j] < grid[i + 1, j];
                    }

                    if (i > 0)
                    {
                        result &= grid[i, j] < grid[i - 1, j];
                    }

                    if (j < sizej - 1)
                    {
                        result &= grid[i, j] < grid[i, j + 1];
                    }

                    if (j > 0)
                    {
                        result &= grid[i, j] < grid[i, j - 1];
                    }

                    if (result)
                    {
                        lowpoints.Add(grid[i, j] + 1);
                    }

                }
            }


            return lowpoints.Sum();
        }

        public object PartTwo(string input)
        {
            int sizei = 100;
            int sizej = 100;
            var grid = new int[sizei, sizej];
            var lines = input.ReadLinesToType<string>().ToList();

            for (int i = 0; i < sizei; i++)
            {
                for (int j = 0; j < sizej; j++)
                {
                    grid[i, j] = int.Parse(lines[i][j].ToString());
                }
            }


            int[,] basinSizes = new int[sizei, sizej];

            for (int y = 0; y < sizej; y++)
            {
                for (int x = 0; x < sizei; x++)
                {
                    int currentX = x;
                    int currentY = y;
                    int currentHeight = grid[currentX, currentY];
                    if (currentHeight == 9) continue; // Skip borders
                    (int downhillX, int downhillY) = LowestNeighbor(x, y);

                    // Walk downhill
                    while (grid[downhillX, downhillY] < currentHeight)
                    {
                        currentX = downhillX;
                        currentY = downhillY;
                        currentHeight = grid[currentX, currentY];
                        (downhillX, downhillY) = LowestNeighbor(currentX, currentY);
                    }

                    basinSizes[currentX, currentY]++;
                }
            }

            return basinSizes.OfType<int>().OrderByDescending(x => x).Take(3).Aggregate(1, (a, b) => a * b);

        }



        (int x, int y) LowestNeighbor(int x, int y)
        {
            int min = int.MaxValue;
            int minX = x;
            int minY = y;
            int val;

            if (x > 0 && (val = grid[x - 1, y]) < min)
            {
                min = val;
                minX = x - 1;
                minY = y;
            }

            if (y > 0 && (val = grid[x, y - 1]) < min)
            {
                min = val;
                minX = x;
                minY = y - 1;
            }

            if (x < sizei - 1 && (val = grid[x + 1, y]) < min)
            {
                min = val;
                minX = x + 1;
                minY = y;
            }

            if (y < sizej - 1 && (val = grid[x, y + 1]) < min)
            {
                min = val;
                minX = x;
                minY = y + 1;
            }

            return (minX, minY);
        }

    }
}
