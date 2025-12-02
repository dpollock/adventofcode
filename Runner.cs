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

        var dayDir = Path.GetDirectoryName(inputPath)!;
        Directory.CreateDirectory(dayDir);

        if (File.Exists(inputPath))
        {
            input = await File.ReadAllTextAsync(inputPath);
            input = input.TrimEnd('\n');
        }
        else
        {
            Console.WriteLine("Fetching input...");
            input = await client.GetInputAsync(solver.Year, solver.Day);
            await File.WriteAllTextAsync(inputPath, input);
        }

        // Fetch/update problem text
        var problemPath = Path.Combine(dayDir, "problem.md");
        Console.WriteLine("Fetching problem...");
        var problem = await client.GetProblemAsync(solver.Year, solver.Day);
        await File.WriteAllTextAsync(problemPath, problem);

        // Part 1
        var sw = Stopwatch.StartNew();
        var result1 = solver.Part1(input);
        sw.Stop();
        PrintResult(1, result1, sw.ElapsedMilliseconds);

        if (submit && result1 > 0)
        {
            var response = await client.SubmitAnswerAsync(solver.Year, solver.Day, 1, result1.ToString());
            Console.WriteLine($"  {response}");
        }

        // Part 2
        sw.Restart();
        var result2 = solver.Part2(input);
        sw.Stop();
        PrintResult(2, result2, sw.ElapsedMilliseconds);

        if (submit && result2 > 0)
        {
            var response = await client.SubmitAnswerAsync(solver.Year, solver.Day, 2, result2.ToString());
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

    private static void PrintResult(int part, long result, long ms)
    {
        if (result < 0)
            Console.WriteLine($"Part {part}: (not implemented)");
        else if (result == 0)
            Console.WriteLine($"Part {part}: 0 ({ms}ms) ⚠️  zero result - verify this is correct");
        else
            Console.WriteLine($"Part {part}: {result} ({ms}ms)");
    }
}
