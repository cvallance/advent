using System.Diagnostics;

namespace DayX;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var sw = Stopwatch.StartNew();
        var stuff = ParseStuff(lines);
        Console.WriteLine($"Parsing {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        var part1 = Part1(stuff);
        Console.WriteLine($"Part1 {sw.ElapsedMilliseconds}ms");
        
        sw.Restart();
        var part2 = Part2(stuff);
        Console.WriteLine($"Part2 {sw.ElapsedMilliseconds}ms");
        
        return (part1, part2);
    }

    private static IList<string> ParseStuff(IList<string> lines)
    {
        return lines;
    }

    private static int Part1(IList<string> stuff)
    {
        return 1;
    }

    private static int Part2(IList<string> stuff)
    {
        return 1;
    }
}