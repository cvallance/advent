var lines = File.ReadLines("../../inputs/day1.txt").ToList();
var elves = new List<int>();

var currentElve = 0;
foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        elves.Add(currentElve);
        currentElve = 0;
        continue;
    }

    currentElve += int.Parse(line);
}

if (currentElve > 0) elves.Add(currentElve);

Console.WriteLine($"Part 1: {elves.Max()}");
Console.WriteLine($"Part 2: {elves.OrderByDescending(x => x).Take(3).Sum()}");
