namespace AdventOfCode;

public interface ISolver
{
    int Year { get; }
    int Day { get; }
    string Name { get; }

    long Part1(string input);
    long Part2(string input);
}

public abstract class Solver(int year, int day, string name) : ISolver
{
    public int Year => year;
    public int Day => day;
    public string Name => name;

    public abstract long Part1(string input);
    public virtual long Part2(string input) => -1;
}
