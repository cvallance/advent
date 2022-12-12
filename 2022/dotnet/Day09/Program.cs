﻿using Day09;

var data = File.ReadAllText("../../inputs/Day09.txt").TrimEnd();

var (part1, part2) = Solver.Solve(data);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
