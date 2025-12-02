using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode;

public static class Runner
{
    public static async Task RunAsync(ISolver solver, AocClient client, bool submit = false)
    {
        Console.WriteLine($"Day {solver.Day}: {solver.Name}");
        Console.WriteLine(new string('-', 40));

        var inputPath = GetInputPath(solver.Year, solver.Day);
        string input;

        if (File.Exists(inputPath))
        {
            input = await File.ReadAllTextAsync(inputPath);
            input = input.TrimEnd('\n');
        }
        else
        {
            Console.WriteLine("Fetching input...");
            input = await client.GetInputAsync(solver.Year, solver.Day);
            Directory.CreateDirectory(Path.GetDirectoryName(inputPath)!);
            await File.WriteAllTextAsync(inputPath, input);
        }

        // Part 1
        var sw = Stopwatch.StartNew();
        var result1 = solver.Part1(input);
        sw.Stop();
        Console.WriteLine($"Part 1: {result1} ({sw.ElapsedMilliseconds}ms)");

        if (submit && result1.ToString() != "Not implemented")
        {
            var response = await client.SubmitAnswerAsync(solver.Year, solver.Day, 1, result1.ToString()!);
            Console.WriteLine($"  {response}");
        }

        // Part 2
        sw.Restart();
        var result2 = solver.Part2(input);
        sw.Stop();
        Console.WriteLine($"Part 2: {result2} ({sw.ElapsedMilliseconds}ms)");

        if (submit && result2.ToString() != "Not implemented")
        {
            var response = await client.SubmitAnswerAsync(solver.Year, solver.Day, 2, result2.ToString()!);
            Console.WriteLine($"  {response}");
        }

        Console.WriteLine();
    }

    public static ISolver? FindSolver(int year, int day)
    {
        var solverType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(ISolver).IsAssignableFrom(t))
            .FirstOrDefault(t =>
            {
                var instance = Activator.CreateInstance(t) as ISolver;
                return instance?.Year == year && instance?.Day == day;
            });

        return solverType != null ? Activator.CreateInstance(solverType) as ISolver : null;
    }

    private static string GetInputPath(int year, int day)
        => Path.Combine(Environment.CurrentDirectory, $"{year}", $"Day{day:D2}", "input.txt");
}
