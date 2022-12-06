using DayX;

var data = File.ReadAllText("../../inputs/DayX.txt");

var (part1, part2) = Solver.Solve(data);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
