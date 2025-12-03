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

        // Fetch problem text and check completion status
        var problemPath = Path.Combine(dayDir, "problem.md");
        Console.WriteLine("Fetching problem...");
        var (problem, solvedParts) = await client.GetProblemAsync(solver.Year, solver.Day);
        await File.WriteAllTextAsync(problemPath, problem);

        // Run samples first
        if (solver.Samples.Length > 0)
        {
            Console.WriteLine("Samples:");
            var allPassed = true;
            foreach (var (sampleInput, expected1, expected2) in solver.Samples)
            {
                if (expected1.HasValue)
                {
                    var actual1 = solver.Part1(sampleInput);
                    var pass1 = actual1 == expected1.Value;
                    Console.WriteLine($"  Part 1: {actual1} {(pass1 ? "✓" : $"✗ expected {expected1}")}");
                    allPassed &= pass1;
                }
                if (expected2.HasValue)
                {
                    var actual2 = solver.Part2(sampleInput);
                    var pass2 = actual2 == expected2.Value;
                    Console.WriteLine($"  Part 2: {actual2} {(pass2 ? "✓" : $"✗ expected {expected2}")}");
                    allPassed &= pass2;
                }
            }
            if (!allPassed)
            {
                Console.WriteLine("Sample validation failed - skipping real input\n");
                return;
            }
            Console.WriteLine();
        }

        // Part 1
        solver.Part1(input); // Warmup
        var times1 = new double[5];
        var sw = new Stopwatch();
        long result1 = 0;
        for (int i = 0; i < 5; i++)
        {
            sw.Restart();
            result1 = solver.Part1(input);
            sw.Stop();
            times1[i] = sw.Elapsed.TotalMilliseconds;
        }
        PrintResult(1, result1, times1.Average(), solvedParts >= 1);

        if (submit && result1 > 0 && solvedParts < 1)
        {
            var response = await client.SubmitAnswerAsync(solver.Year, solver.Day, 1, result1.ToString());
            Console.WriteLine($"  {response}");
            if (response.Contains("Correct"))
            {
                // Refresh problem to get part 2
                Console.WriteLine("Fetching part 2...");
                (problem, solvedParts) = await client.GetProblemAsync(solver.Year, solver.Day);
                await File.WriteAllTextAsync(problemPath, problem);
            }
        }

        // Part 2
        solver.Part2(input); // Warmup
        var times2 = new double[5];
        long result2 = 0;
        for (int i = 0; i < 5; i++)
        {
            sw.Restart();
            result2 = solver.Part2(input);
            sw.Stop();
            times2[i] = sw.Elapsed.TotalMilliseconds;
        }
        PrintResult(2, result2, times2.Average(), solvedParts >= 2);

        if (submit && result2 > 0 && solvedParts < 2)
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

    private static void PrintResult(int part, long result, double ms, bool alreadySolved)
    {
        var solved = alreadySolved ? " ✓" : "";
        if (result < 0)
            Console.WriteLine($"Part {part}: (not implemented){solved}");
        else if (result == 0)
            Console.WriteLine($"Part {part}: 0 ({ms:F3}ms) ⚠️  zero result{solved}");
        else
            Console.WriteLine($"Part {part}: {result} ({ms:F3}ms){solved}");
    }
}
