var lines = File.ReadLines("../../inputs/day3.txt").ToList();

int Priority(char item)
{
    var asciiValue = (byte)item;
    if (asciiValue > 90) return asciiValue - 96;
    return asciiValue - 38;
}

var part1 = 0;

var part2 = 0;
var elf = 0;
var sharedItems = new List<char>();
foreach (var line in lines)
{
    // Part 1
    var midPoint = line.Length / 2;
    var firstComp = line.Substring(0, midPoint);
    var secondComp = line.Substring(midPoint);
    var part1Items = new HashSet<char>();
    foreach (var item in firstComp) part1Items.Add(item);
    foreach (var item in secondComp)
    {
        if (part1Items.Contains(item))
        {
            part1 += Priority(item);
            break;
        }
    }
    
    // Part 2
    if (elf % 3 == 0)
    {
        sharedItems = new List<char>(line);
        elf += 1;
        continue;
    }

    sharedItems = sharedItems.Intersect(new List<char>(line)).ToList();
    
    if (elf % 3 == 2) part2 += Priority(sharedItems.First());

    elf += 1;
}

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
