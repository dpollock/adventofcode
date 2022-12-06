using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day02;

[ProblemName("Rock Paper Scissors")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var rounds = input.ReadLinesToObject<TournamentRoundPart1>();

        var result = rounds.Sum(r => r.ChoicePoints + r.WinPoints);
        //elf with the most calories
        return result;
    }

    public object PartTwo(string input)
    {
        var rounds = input.ReadLinesToObject<TournamentRoundPart2>();

        var result = rounds.Sum(r => r.ChoicePoints + r.WinPoints);
        //elf with the most calories
        return result;
    }
}

internal class TournamentRoundPart1
{
    public string OpponentChoice { get; set; }
    public string YourChoice { get; set; }

    public int WinPoints
    {
        get
        {
            switch (OpponentChoice)
            {
                case "A" when YourChoice == "X":
                case "B" when YourChoice == "Y":
                case "C" when YourChoice == "Z":
                    return 3;
                case "A" when YourChoice == "Y":
                case "B" when YourChoice == "Z":
                case "C" when YourChoice == "X":
                    return 6;
                default:
                    return 0;
            }
        }
    }

    public int ChoicePoints
    {
        get
        {
            return YourChoice switch
            {
                "X" =>
                    1,
                "Y" =>
                    2,
                "Z" =>
                    3,
                _ => 0
            };
        }
    }
}

internal class TournamentRoundPart2
{
    public string OpponentChoice { get; set; }
    public string ExpectedOutCome { get; set; }

    public string YourChoice
    {
        get
        {
            switch (ExpectedOutCome)
            {
                case "X" when OpponentChoice == "A":
                case "Y" when OpponentChoice == "C":
                case "Z" when OpponentChoice == "B":
                    return "Z";

                case "X" when OpponentChoice == "B":
                case "Y" when OpponentChoice == "A":
                case "Z" when OpponentChoice == "C":
                    return "X";

                case "X" when OpponentChoice == "C":
                case "Y" when OpponentChoice == "B":
                case "Z" when OpponentChoice == "A":
                    return "Y";

                default:
                    return "";
            }
        }
    }


    public int WinPoints
    {
        get
        {
            switch (OpponentChoice)
            {
                case "A" when YourChoice == "X":
                case "B" when YourChoice == "Y":
                case "C" when YourChoice == "Z":
                    return 3;
                case "A" when YourChoice == "Y":
                case "B" when YourChoice == "Z":
                case "C" when YourChoice == "X":
                    return 6;
                default:
                    return 0;
            }
        }
    }

    public int ChoicePoints
    {
        get
        {
            return YourChoice switch
            {
                "X" =>
                    1,
                "Y" =>
                    2,
                "Z" =>
                    3,
                _ => 0
            };
        }
    }
}