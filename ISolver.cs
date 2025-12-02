namespace AdventOfCode;

public interface ISolver
{
    int Year { get; }
    int Day { get; }
    string Name { get; }

    object Part1(string input);
    object Part2(string input);
}

public abstract class Solver(int year, int day, string name) : ISolver
{
    public int Year => year;
    public int Day => day;
    public string Name => name;

    public abstract object Part1(string input);
    public virtual object Part2(string input) => "Not implemented";
}
