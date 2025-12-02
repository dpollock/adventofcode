using AdventOfCode;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var session = config["AocSession"] ?? Environment.GetEnvironmentVariable("AOC_SESSION");

if (string.IsNullOrEmpty(session))
{
    Console.WriteLine("No session token found.");
    Console.WriteLine("Set it via: dotnet user-secrets set AocSession <your-session-cookie>");
    Console.WriteLine("Or set AOC_SESSION environment variable.");
    return 1;
}

var client = new AocClient(session);

// Parse arguments - defaults to today
var year = DateTime.Now.Year;
var day = DateTime.Now.Day;
var submit = false;

for (int i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "-s" or "--submit":
            submit = true;
            break;
        case var arg when arg.Contains('/'):
            var parts = arg.Split('/');
            year = int.Parse(parts[0]);
            day = int.Parse(parts[1]);
            break;
        case var arg when int.TryParse(arg, out var d):
            day = d;
            break;
    }
}

var solver = Runner.FindSolver(year, day);
if (solver == null)
{
    Console.WriteLine($"No solver found for {year}/Day{day:D2} - creating scaffold...");
    await Scaffold.CreateAsync(year, day, client);
    Console.WriteLine($"Created {year}/Day{day:D2}/Solution.cs");
    Console.WriteLine("Run again after implementing your solution.");
    return 0;
}

await Runner.RunAsync(solver, client, submit);
return 0;

public partial class Program { }
