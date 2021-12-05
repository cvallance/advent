var lines = File.ReadLines("../../inputs/day1.txt").ToList();
var numbers = lines.Select(int.Parse).ToList();

// First
var last = int.MaxValue;
var higherCount = 0;
foreach (var number in numbers)
{
    if (number > last) higherCount += 1;
    last = number;
}

Console.WriteLine(higherCount); // 1477

// Second
last = int.MaxValue;
higherCount = 0;
for (var i = 0; i <= numbers.Count - 3; i++)
{
    var number = numbers[i] + numbers[i + 1] + numbers[i + 2];
    if (number > last) higherCount += 1;
    last = number;
}

Console.WriteLine(higherCount); // 1523
