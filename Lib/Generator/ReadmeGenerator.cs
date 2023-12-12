using AdventOfCode.Model;

namespace AdventOfCode.Generator;

internal class ProjectReadmeGenerator
{
    public string Generate(int firstYear, int lastYear)
    {
        return $@"
           > # Advent of Code ({firstYear}-{lastYear})
           > C# solutions to the Advent of Code problems.
           > Check out https://adventofcode.com.
           > 
           > <a href=""https://adventofcode.com""><img src=""{lastYear}/calendar.svg"" width=""80%"" /></a>
           > 
           > 
           > ## Dependencies

           > - This project is based on `.NET 8`  and `C# 10`. It should work on Windows, Linux and OS-X.
           > - `AngleSharp` is used for problem download.

           > ## Running

           > To run the project:

           > 1. Install .NET Core
           > 2. Clone the repo
           > 3. Get help with `dotnet run`
           > ```
           > {Usage.Get()}
           > ```
           > ".StripMargin("> ");
    }
}

internal class ReadmeGeneratorForYear
{
    public string Generate(Calendar calendar)
    {
        return $@"
           > # Advent of Code ({calendar.Year})
           > Check out https://adventofcode.com/{calendar.Year}.

           > <a href=""https://adventofcode.com/{calendar.Year}""><img src=""calendar.svg"" width=""80%"" /></a>
           
           > ".StripMargin("> ");
    }
}