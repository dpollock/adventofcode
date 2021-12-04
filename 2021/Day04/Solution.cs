using adventofcode.Lib;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2021.Day04
{
    [ProblemName("Giant Squid")]
    class Solution : ISolver
    {

        public object PartOne(string input)
        {
            var rows = input.ReadLinesToType<string>();
            var numbers = rows.First().Split(",").ToList().Select(int.Parse);

            var boards = ParseBoards(rows);

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    board.MarkBoard(number);
                }

                var winningBoard = boards.FirstOrDefault(b => b.IsWinner());
                if (winningBoard != null)
                    return winningBoard.SumOfUnMarked() * number;
            }


            return 0;
        }




        public object PartTwo(string input)
        {
            var rows = input.ReadLinesToType<string>();
            var numbers = rows.First().Split(",").ToList().Select(int.Parse);

            var boards = ParseBoards(rows);
            Board lastWinningBoard = null;
            int lastWinningNumber = 0;

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    board.MarkBoard(number);
                }

                var winningBoards = boards.Where(b => b.IsWinner()).ToList();

                if (winningBoards.Count() > 1)
                {
                    int x = 5;
                }

                if (winningBoards.Any())
                {
                    lastWinningBoard = winningBoards.FirstOrDefault();
                    lastWinningNumber = number;
                    winningBoards.ForEach(x=> boards.Remove(x));
                }


            }

            var sumOfUnMarked = lastWinningBoard.SumOfUnMarked() -  lastWinningNumber; 
            
            return sumOfUnMarked * lastWinningNumber;
        }

        private List<Board> ParseBoards(IEnumerable<string> input)
        {
            List<Board> boards = new List<Board>();
            Board board = new Board();
            int boardRow = 0;
            foreach (var i in input.Skip(1))
            {
                if (string.IsNullOrEmpty(i))
                {
                    boardRow = 0;
                    board = new Board();
                    boards.Add(board);
                    continue;
                }

                var test = i.Split(" ").Where(x => x.Length > 0).Select(x => int.Parse(x.Trim())).ToList();
                for (int j = 0; j < test.Count; j++)
                {
                    board.Cells[boardRow, j] = new Cell() { Number = test[j] };
                }

                boardRow++;
            }

            return boards;
        }


        private class Board
        {
            public Cell[,] Cells { get; set; } = new Cell[5, 5];

            public bool IsWinner()
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Cells[i, 0].Marked &&
                        Cells[i, 1].Marked &&
                        Cells[i, 2].Marked &&
                        Cells[i, 3].Marked &&
                        Cells[i, 4].Marked)
                    {
                        return true;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    if (Cells[0, i].Marked &&
                        Cells[1, i].Marked &&
                        Cells[2, i].Marked &&
                        Cells[3, i].Marked &&
                        Cells[4, i].Marked)
                    {
                        return true;
                    }
                }

                return false;
            }

            public int SumOfUnMarked()
            {
                var result = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (!Cells[i, j].Marked)
                            result += Cells[i, j].Number;
                    }
                }

                return result;
            }

            public void MarkBoard(int number, bool value = true)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (Cells[i, j].Number == number)
                            Cells[i, j].Marked = value;
                    }
                }
            }
        }

        private class Cell
        {
            public int Number { get; set; }
            public bool Marked { get; set; }
        }
    }
}
