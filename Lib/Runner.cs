using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode;

internal class ProblemName : Attribute
{
    public readonly string Name;

    public ProblemName(string name)
    {
        Name = name;
    }
}

internal interface ISolver
{
    object PartOne(string input)
    {
        return null;
    }

    object PartTwo(string input)
    {
        return null;
    }

    (long, long) Solve(string input)
    {
        return (long.Parse(PartOne(input).ToString() ?? "0"), long.Parse(PartTwo(input).ToString() ?? "0"));
    }

    (long, long, long, long) SolveSample()
    {
        return (0, 0, 0, 0);
    }
}

internal static class SolverExtensions
{

    public static string GetName(this ISolver solver)
    {
        return (
            solver
                .GetType()
                .GetCustomAttribute(typeof(ProblemName)) as ProblemName
        ).Name;
    }

    public static string DayName(this ISolver solver)
    {
        return $"Day {solver.Day()}";
    }

    public static int Year(this ISolver solver)
    {
        return Year(solver.GetType());
    }

    public static int Year(Type t)
    {
        return int.Parse(t.FullName.Split('.')[1].Substring(1));
    }

    public static int Day(this ISolver solver)
    {
        return Day(solver.GetType());
    }

    public static int Day(Type t)
    {
        return int.Parse(t.FullName.Split('.')[2].Substring(3));
    }

    public static string WorkingDir(int year)
    {
        string workingDirectory = Environment.CurrentDirectory;
        // string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
        return Path.Combine(workingDirectory, year.ToString());
    }

    public static string WorkingDir(int year, int day)
    {
        return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
    }

    public static string WorkingDir(this ISolver solver)
    {
        return WorkingDir(solver.Year(), solver.Day());
    }

    public static ISplashScreen SplashScreen(this ISolver solver)
    {
        var tsplashScreen = Assembly.GetEntryAssembly().GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && typeof(ISplashScreen).IsAssignableFrom(t))
            .Single(t => Year(t) == solver.Year());
        return (ISplashScreen)Activator.CreateInstance(tsplashScreen);
    }
}

internal record SolverResult(long[] answers, string[] errors);

internal class Runner
{
    private static string GetNormalizedInput(string file)
    {
        var input = File.ReadAllText(file);
        if (input.EndsWith("\n")) input = input.Substring(0, input.Length - 1);
        return input;
    }

    public static SolverResult RunSampleSolver(ISolver solver)
    {
        Write(ConsoleColor.White, $"\t{solver.DayName()}: (Sample) {solver.GetName()}");
        WriteLine();
        var errors = new List<string>();
        var stopwatch = Stopwatch.StartNew();

        var (part1, part2, part1Answer, part2Answer) = solver.SolveSample();
        var ticks = stopwatch.ElapsedTicks;


        long[] refout2 = { part1Answer, part2Answer };
        long[] answers = { part1, part2 };
        CheckAnswerLocally(solver.DayName(), refout2, 1, part1, out var status, out var err);
        if (err != null) errors.Add(err);

        CheckAnswerLocally(solver.DayName(), refout2, 2, part2, out status, out err);
        if (err != null) errors.Add(err);

        var diff = ticks * 1000.0 / Stopwatch.Frequency;

        WriteLine();
        WriteLine(
            diff > 1000 ? ConsoleColor.Red :
            diff > 500 ? ConsoleColor.Yellow :
            ConsoleColor.DarkGreen,
            $"\t  ({diff:F3} ms)"
        );
        stopwatch.Restart();

        return new SolverResult(answers.ToArray(), errors.ToArray());
    }

    public static SolverResult RunSolver(ISolver solver)
    {
        var workingDir = solver.WorkingDir();
        Write(ConsoleColor.White, $"\t{solver.DayName()}: {solver.GetName()}");
        WriteLine();
        var file = Path.Combine(workingDir, "input.in");
        var refoutFile = file.Replace(".in", ".refout");
        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile).Select(long.Parse).ToArray() : null;
        var input = GetNormalizedInput(file);
        var errors = new List<string>();
        var stopwatch = Stopwatch.StartNew();

        var (part1, part2) = solver.Solve(input);
        var ticks = stopwatch.ElapsedTicks;


        CheckAnswerLocally(solver.DayName(), refout, 1, part1, out var status, out var err);
        if (err != null) errors.Add(err);

        CheckAnswerLocally(solver.DayName(), refout, 2, part2, out status, out err);
        if (err != null) errors.Add(err);

        var diff = ticks * 1000.0 / Stopwatch.Frequency;

        WriteLine();
        WriteLine(
            diff > 1000 ? ConsoleColor.Red :
            diff > 500 ? ConsoleColor.Yellow :
            ConsoleColor.DarkGreen,
            $"\t  ({diff:F3} ms)"
        );
        stopwatch.Restart();

        return new SolverResult(new[] { part1, part2 }, errors.ToArray());
    }

    private static void CheckAnswerLocally(string name, long[] refout, int part, long answer, out string status,
        out string err)
    {
        (var statusColor, status, err) =
            refout == null || refout.Length <= part - 1 ? (ConsoleColor.Cyan, "?", null) :
            refout[part - 1] == answer ? (ConsoleColor.DarkGreen, "✓", null) :
            (ConsoleColor.Red, "X",
                $"{name}: In line {part} expected '{refout[part - 1]}' but found '{answer}'");

        Write(statusColor, $"\t{status}");
        Console.WriteLine($" {answer} ");
    }

    public static void RunAll(params ISolver[] solvers)
    {
        var errors = new List<string>();

        var lastYear = -1;
        foreach (var solver in solvers)
        {
            if (lastYear != solver.Year())
            {
                solver.SplashScreen().Show();
                lastYear = solver.Year();
            }

            var sampleResult = RunSampleSolver(solver);
            WriteLine();
            errors.AddRange(sampleResult.errors);
            WriteLine();
            var result = RunSolver(solver);
            WriteLine();
            errors.AddRange(result.errors);
        }

        WriteLine();

        if (errors.Any()) WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));

        // Console.ReadLine();
    }

    private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        Write(color, text + "\n");
    }

    private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        var c = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = c;
    }
}