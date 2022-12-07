namespace Day07;

public static class Solver
{
    public static (long, long) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var part1 = 0L;
        var total = 70_000_000;
        var updateSize = 30_000_000;

        var root = ParseDirectoryTree(lines);
        var rootSize = root.GetSize();
        var currentFree = total - rootSize;
        var needed = updateSize - currentFree;
        var flattened = root.GetFlatChildren();
        foreach (var directory in flattened)
        {
            if (directory.GetSize() <= 100000) part1 += directory.GetSize();
        }

        var part2 = flattened.Where(x => x.GetSize() > needed).OrderBy(x => x.GetSize()).First().GetSize();
        return (part1, part2);
    }

    public static Directory ParseDirectoryTree(List<string> lines)
    {
        var root = new Directory("root");
        var current = root;

        foreach (var line in lines)
        {
            switch (line)
            {
                case var _ when line.StartsWith("$ cd "):
                    var directory = line[5..];
                    current = directory switch
                    {
                        "/" => root,
                        ".." => current.Parent!,
                        _ => current.SubDirectories.First(x => x.Name == directory)
                    };
                    break;
                case "$ ls":
                    // Nothing to do here 
                    break;
                default:
                    if (string.IsNullOrWhiteSpace(line)) break;
                    if (line.StartsWith("dir "))
                    {
                        current.SubDirectories.Add(new Directory(line[4..], current));
                    }
                    else
                    {
                        var parts = line.Split(" ");
                        current.Files.Add(new File(long.Parse(parts[0]), parts[1]));
                    }
                    break;
            }
        }
        return root;
    }

    public record Directory(string Name, Directory? Parent = null)
    {
        public IList<Directory> SubDirectories { get; } = new List<Directory>();
        public IList<File> Files { get; } = new List<File>();

        public long GetSize()
        {
            return SubDirectories.Sum(x => x.GetSize()) + Files.Sum(x => x.Size);
        }

        public List<Directory> GetFlatChildren()
        {
            var allChildren = SubDirectories.SelectMany(x => x.GetFlatChildren());
            return SubDirectories.Concat(allChildren).ToList();
        }
    }

    public record File(long Size, string Name);
}