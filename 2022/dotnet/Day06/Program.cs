using Day06;

var data = File.ReadAllText("../../inputs/Day06.txt");

var (part1, part2) = Solver.Solve(data);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
