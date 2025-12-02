namespace AdventOfCode;

public static class Scaffold
{
    public static async Task CreateAsync(int year, int day, AocClient client)
    {
        var dayDir = Path.Combine(Environment.CurrentDirectory, $"{year}", $"Day{day:D2}");
        Directory.CreateDirectory(dayDir);

        // Fetch input
        Console.WriteLine("Fetching input...");
        var input = await client.GetInputAsync(year, day);
        await File.WriteAllTextAsync(Path.Combine(dayDir, "input.txt"), input);

        // Fetch problem
        Console.WriteLine("Fetching problem...");
        var (problem, _) = await client.GetProblemAsync(year, day);
        await File.WriteAllTextAsync(Path.Combine(dayDir, "problem.md"), problem);

        // Create solution file
        var solutionPath = Path.Combine(dayDir, "Solution.cs");
        if (!File.Exists(solutionPath))
        {
            var template = $$"""
                namespace AdventOfCode.Y{{year}};

                public class Day{{day:D2}}() : Solver({{year}}, {{day}}, "")
                {
                    public override (string input, long? expected1, long? expected2)[] Samples =>
                    [
                        // ("sample", expected1, expected2),
                    ];

                    public override long Part1(string input)
                    {
                        var lines = Parse.Lines(input);

                        return -1;
                    }

                    public override long Part2(string input)
                    {
                        var lines = Parse.Lines(input);

                        return -1;
                    }
                }
                """;
            await File.WriteAllTextAsync(solutionPath, template);
        }
    }
}
