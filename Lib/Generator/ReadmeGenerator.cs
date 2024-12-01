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
           >
           > ### VS Code Tasks
           > This project includes VS Code tasks for common operations:
           > 
           > - **Run Solution**: Execute a specific day's solution
           > - **Update Input**: Download input for a specific day
           > - **Upload Solution**: Submit your answer for a specific day
           > 
           > To use these tasks:
           > 1. Open the Command Palette (Ctrl+Shift+P)
           > 2. Type 'Tasks: Run Task'
           > 3. Select the desired task
           > 4. Enter the year and day when prompted
           > 
           > Note: For downloading inputs and uploading solutions, you'll need your session cookie from adventofcode.com
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