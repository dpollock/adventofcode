using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day22;

[ProblemName("Monkey Map")]
internal class Solution : ISolver
{
    public int dimx, dimy, dimc;

    public char[,] mapp = new char[0, 0];
    public char[] turns = { };
    public int[] xmax = { }, xmin = { }, ymax = { }, ymin = { }, moves = { };

    public object PartOne(string input)
    {
        Parse(input.ReadLinesToType<string>().ToList());


        (int, int) Move(int x, int y, int dx, int dy)
        {
            int nx = x + dx, ny = y + dy;
            // wrap around
            if (dx != 0)
            {
                if (nx > xmax[ny]) nx = xmin[ny];
                if (nx < xmin[ny]) nx = xmax[ny];
            }
            else
            {
                if (ny > ymax[nx]) ny = ymin[nx];
                if (ny < ymin[nx]) ny = ymax[nx];
            }

            return (nx, ny);
        }

        int x = xmin[0], y = 0, dx = 1, dy = 0;
        for (var i = 0; i < moves.Length; i++)
        {
            for (var j = 0; j < moves[i]; j++)
            {
                var (nx, ny) = Move(x, y, dx, dy);
                if (mapp[ny, nx] == '#') break;
                (x, y) = (nx, ny);
            }

            if (i < turns.Length) (dx, dy) = turns[i] == 'R' ? (-dy, dx) : (dy, -dx);
        }

        var face = dx != 0 ? -(dx - 1) : 2 - dy;
        var result = 1000 * (y + 1) + 4 * (x + 1) + face;
        return result;
    }

    public object PartTwo(string input)
    {
        Parse(input.ReadLinesToType<string>().ToList());
        int x = xmin[0], y = 0, dx = 1, dy = 0;
        for (var i = 0; i < moves.Length; i++)
        {
            for (var j = 0; j < moves[i]; j++)
            {
                var (nx, ny, ndx, ndy) = Move(x, y, dx, dy);
                if (mapp[ny, nx] == '#') break;
                (x, y, dx, dy) = (nx, ny, ndx, ndy);
            }

            if (i < turns.Length) (dx, dy) = turns[i] == 'R' ? (-dy, dx) : (dy, -dx);
        }

        var face = dx != 0 ? -(dx - 1) : 2 - dy;
        var result = 1000 * (y + 1) + 4 * (x + 1) + face;
        return result;
    }

    public void Parse(List<string> input)
    {
        var directions = input[^1];
        dimy = input.Count - 3;
        dimx = input.GetRange(0, input.Count - 2).Select(s => s.Length).Max() - 1;
        dimc = (dimy + 1) / 4;
        mapp = new char[dimy + 1, dimx + 1];
        xmax = new int[dimy + 1];
        ymax = new int[dimx + 1];
        xmin = new int[dimy + 1];
        ymin = new int[dimx + 1];
        for (var y = 0; y <= dimy; y++)
        for (var x = 0; x <= dimx; x++)
            mapp[y, x] = x < input[y].Length ? input[y][x] : ' ';
        for (var y = 0; y <= dimy; y++)
        for (var x = 0; x <= dimx; x++)
            if (mapp[y, x] != ' ')
            {
                xmin[y] = x;
                break;
            }

        for (var y = 0; y <= dimy; y++)
        for (var x = 0; x <= dimx; x++)
            if (mapp[y, dimx - x] != ' ')
            {
                xmax[y] = dimx - x;
                break;
            }

        for (var x = 0; x <= dimx; x++)
        for (var y = 0; y <= dimy; y++)
            if (mapp[y, x] != ' ')
            {
                ymin[x] = y;
                break;
            }

        for (var x = 0; x <= dimx; x++)
        for (var y = 0; y <= dimy; y++)
            if (mapp[dimy - y, x] != ' ')
            {
                ymax[x] = dimy - y;
                break;
            }

        moves = Regex.Split(directions, "[LR]").Select(int.Parse).ToArray();
        turns = new char[moves.Length - 1];
        for (int i = 0, j = 0; i < directions.Length; i++)
            if (directions[i] == 'L' || directions[i] == 'R')
                turns[j++] = directions[i];
    }

    


    protected (int, int, int, int) Move(int x, int y, int dx, int dy)
    {
        int nx = x + dx, ny = y + dy, ndx = dx, ndy = dy;
        // wrap around
        if (dx != 0)
        {
            if (nx > xmax[ny])
            {
                if (ny < dimc)
                {
                    // B->E+RR
                    nx = 2 * dimc - 1;
                    ny = 3 * dimc - 1 - ny;
                    ndx = -dx;
                }
                else if (ny < 2 * dimc)
                {
                    // C->B+L
                    nx = ny + dimc;
                    ny = dimc - 1;
                    (ndx, ndy) = (dy, -dx);
                }
                else if (ny < 3 * dimc)
                {
                    // E->B+RR
                    nx = 3 * dimc - 1;
                    ny = 3 * dimc - 1 - ny;
                    ndx = -dx;
                }
                else
                {
                    // F->E+L
                    nx = ny - 2 * dimc;
                    ny = 3 * dimc - 1;
                    (ndx, ndy) = (dy, -dx);
                }
            }
            else if (nx < xmin[ny])
            {
                if (ny < dimc)
                {
                    // A->D+RR
                    nx = 0;
                    ny = 3 * dimc - 1 - ny;
                    ndx = -dx;
                }
                else if (ny < 2 * dimc)
                {
                    // C->D+L
                    nx = ny - dimc;
                    ny = 2 * dimc;
                    (ndx, ndy) = (dy, -dx);
                }
                else if (ny < 3 * dimc)
                {
                    // D->A+RR
                    nx = dimc;
                    ny = 3 * dimc - 1 - ny;
                    ndx = -dx;
                }
                else
                {
                    // F->A+L
                    nx = ny - 2 * dimc;
                    ny = 0;
                    (ndx, ndy) = (dy, -dx);
                }
            }
        }
        else
        {
            if (ny > ymax[nx])
            {
                if (nx < dimc)
                {
                    // F->B
                    ny = 0;
                    nx = nx + 2 * dimc;
                }
                else if (nx < 2 * dimc)
                {
                    // E->F+R
                    ny = nx + 2 * dimc;
                    nx = dimc - 1;
                    (ndx, ndy) = (-dy, dx);
                }
                else
                {
                    // B->C+R
                    ny = nx - dimc;
                    nx = 2 * dimc - 1;
                    (ndx, ndy) = (-dy, dx);
                }
            }
            else if (ny < ymin[nx])
            {
                if (nx < dimc)
                {
                    // D->C+R
                    ny = nx + dimc;
                    nx = dimc;
                    (ndx, ndy) = (-dy, dx);
                }
                else if (nx < 2 * dimc)
                {
                    // A->F+R
                    ny = nx + 2 * dimc;
                    nx = 0;
                    (ndx, ndy) = (-dy, dx);
                }
                else
                {
                    // B->F
                    ny = 4 * dimc - 1;
                    nx = nx - 2 * dimc;
                }
            }
        }

        return (nx, ny, ndx, ndy);
    }
}