namespace Day04;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var part1 = 0;
        var part2 = 0;
        var lines = data.Split("\n").ToList();
        foreach (var line in lines)
        {
            var elves = line.Split(",");
            var elf1 = elves[0].Split("-").Select(int.Parse).ToList();
            var elf2 = elves[1].Split("-").Select(int.Parse).ToList();
            if (elf1[0] >= elf2[0] && elf1[1] <= elf2[1]) part1 += 1;
            else if (elf2[0] >= elf1[0] && elf2[1] <= elf1[1]) part1 += 1;

            if (elf1[0] <= elf2[0] && elf2[0] <= elf1[1]) part2 += 1;
            else if (elf2[0] <= elf1[0] && elf1[0] <= elf2[1]) part2 += 1;
        }

        return (part1, part2);
    }
}