using Day6;

// var lines = File.ReadLines("../../inputs/day6-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day6.txt").ToList();
var items = lines[0].Split(",").Select(int.Parse).ToList();

// var first = new FishTwo(items);
// Console.WriteLine(first.Total(80)); // 345793
var second = new FishTwo(items);
Console.WriteLine(second.Total(256)); // 1572643095893
