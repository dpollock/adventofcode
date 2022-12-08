using System.Linq;
using adventofcode.Lib;
using MoreLinq;

namespace AdventOfCode.Y2022.Day08;

[ProblemName("Treetop Tree House")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var size = 99;
        var grid = new Tree[size][];
        var lines = input.ReadLinesToType<string>();

        foreach (var line in lines.Index())
            grid[line.Key] = line.Value.ToCharArray().Index()
                .Select(x => new Tree(line.Key, x.Key, int.Parse(x.Value.ToString()), null)).ToArray();


        for (var row = 0; row < size; row++)
        for (var col = 0; col < size; col++)
        {
            var thisTree = grid[row][col];
            if (row == 0 || col == 0 || row == size - 1 || col == size - 1)
            {
                thisTree.IsVisible = true;
                continue;
            }


            //check down
            var canSee = true;
            for (var tempRow = row + 1; tempRow < size; tempRow++)
                if (grid[tempRow][col].Height >= thisTree.Height)
                {
                    canSee = false;
                    break;
                }

            if (canSee)
            {
                thisTree.IsVisible = true;
                continue;
            }

            //check up
            canSee = true;
            for (var tempRow = row - 1; tempRow >= 0; tempRow--)
                if (grid[tempRow][col].Height >= thisTree.Height)
                {
                    canSee = false;
                    break;
                }

            if (canSee)
            {
                thisTree.IsVisible = true;
                continue;
            }

            //check right
            canSee = true;
            for (var tempCol = col + 1; tempCol < size; tempCol++)
                if (grid[row][tempCol].Height >= thisTree.Height)
                {
                    canSee = false;
                    break;
                }

            if (canSee)
            {
                thisTree.IsVisible = true;
                continue;
            }

            //check left
            canSee = true;
            for (var tempCol = col - 1; tempCol >= 0; tempCol--)
                if (grid[row][tempCol].Height >= thisTree.Height)
                {
                    canSee = false;
                    break;
                }

            thisTree.IsVisible = canSee;
        }


        var visibleTrees = grid.Sum(x => x.Count(t => t.IsVisible == true));
        return visibleTrees;
    }

    public object PartTwo(string input)
    {
        var size = 99;
        var grid = new Tree[size][];
        var lines = input.ReadLinesToType<string>();

        foreach (var line in lines.Index())
            grid[line.Key] = line.Value.ToCharArray().Index()
                .Select(x => new Tree(line.Key, x.Key, int.Parse(x.Value.ToString()), null)).ToArray();


        for (var row = 0; row < size; row++)
        for (var col = 0; col < size; col++)
        {
            var thisTree = grid[row][col];

            //check down
            var downScore = 0;
            for (var tempRow = row + 1; tempRow < size; tempRow++)
            {
                downScore++;
                if (grid[tempRow][col].Height >= thisTree.Height) break;
            }

            //check up
            var upScore = 0;
            for (var tempRow = row - 1; tempRow >= 0; tempRow--)
            {
                upScore++;
                if (grid[tempRow][col].Height >= thisTree.Height) break;
            }

            //check right
            var rightScore = 0;
            for (var tempCol = col + 1; tempCol < size; tempCol++)
            {
                rightScore++;
                if (grid[row][tempCol].Height >= thisTree.Height) break;
            }


            //check left
            var leftScore = 0;
            for (var tempCol = col - 1; tempCol >= 0; tempCol--)
            {
                leftScore++;
                if (grid[row][tempCol].Height >= thisTree.Height) break;
            }

            thisTree.ScenicScore = upScore * downScore * leftScore * rightScore;
        }


        var bestScenicScore = grid.Max(x => x.Max(t => t.ScenicScore));
        return bestScenicScore;
    }
}

internal class Tree
{
    public Tree(int row, int column, int height, bool? isVisible)
    {
        Row = row;
        Column = column;
        Height = height;
        IsVisible = isVisible;
    }

    public int Row { get; init; }
    public int Column { get; init; }
    public int Height { get; init; }
    public bool? IsVisible { get; set; }

    public int ScenicScore { get; set; }
}