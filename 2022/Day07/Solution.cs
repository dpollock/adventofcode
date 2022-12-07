using System.Collections.Generic;
using System.Linq;
using adventofcode.Lib;

namespace AdventOfCode.Y2022.Day07;

[ProblemName("No Space Left On Device")]
internal class Solution : ISolver
{
    public object PartOne(string input)
    {
        var rootDirectory = ParseFile(input);
        var directories = DirectoriesWithSizeAtMost(rootDirectory, 100000);
        var result = directories.Sum(d => d.Size);
        return result;
    }

    public object PartTwo(string input)
    {
        var hardDriveSize = 70000000;
        var freeSizeNeeded = 30000000;

        var rootDirectory = ParseFile(input);

        var currentFreeSpace = hardDriveSize - rootDirectory.Size;
        var needToFreeAtLeast = freeSizeNeeded - currentFreeSpace;

        var directories = DirectoriesWithSizeAtLeast(rootDirectory, needToFreeAtLeast).OrderBy(x => x.Size);

        return directories.First().Size;
    }

    private List<Directory> DirectoriesWithSizeAtMost(Directory dir, int maxSize)
    {
        var result = new List<Directory>();
        foreach (var subDir in dir.Directories)
        {
            if (subDir.Size <= maxSize) result.Add(subDir);
            result.AddRange(DirectoriesWithSizeAtMost(subDir, maxSize));
        }

        return result;
    }

    private List<Directory> DirectoriesWithSizeAtLeast(Directory dir, int maxSize)
    {
        var result = new List<Directory>();
        foreach (var subDir in dir.Directories)
        {
            if (subDir.Size >= maxSize) result.Add(subDir);
            result.AddRange(DirectoriesWithSizeAtLeast(subDir, maxSize));
        }

        return result;
    }

    private Directory ParseFile(string input)
    {
        var rootDirectory = new Directory(null, "/");
        var currentDirectory = rootDirectory;


        var lines = input.ReadLinesToType<string>();
        foreach (var line in lines)
        {
            var values = line.Split(' ');
            switch (values[0])
            {
                case "$":
                    switch (values[1])
                    {
                        case "ls":

                            break;
                        case "cd":
                            switch (values[2])
                            {
                                case "/":
                                    currentDirectory = rootDirectory;
                                    break;
                                case "..":
                                    currentDirectory = currentDirectory?.ParentDirectory;
                                    break;
                                default:
                                    currentDirectory =
                                        currentDirectory.Directories.First(d => d.DirectoryName == values[2]);
                                    break;
                            }

                            break;
                    }

                    break;
                case "dir":
                {
                    var newDirectory = new Directory(currentDirectory, values[1]);
                    currentDirectory.Directories.Add(newDirectory);
                    break;
                }
                default:
                {
                    if (int.TryParse(values[0], out var fileSize))
                    {
                        var newFile = new File(values[1], fileSize);
                        currentDirectory.Files.Add(newFile);
                    }

                    break;
                }
            }
        }

        return rootDirectory;
    }
}

internal record File(string fileName, int Size);

internal class Directory
{
    public Directory(Directory parentDirectory, string directoryName)
    {
        DirectoryName = directoryName;
        ParentDirectory = parentDirectory;
    }

    public Directory ParentDirectory { get; set; }

    public int Size => Files.Sum(f => f.Size) + Directories.Sum(d => d.Size);
    public string DirectoryName { get; set; }
    public List<File> Files { get; set; } = new();
    public List<Directory> Directories { get; set; } = new();
}