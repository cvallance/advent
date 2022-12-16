using Day15;

var data = File.ReadAllText("../../inputs/Day15.txt").TrimEnd();

var (part1, part2) = Solver.Solve(data, 2_000_000);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
