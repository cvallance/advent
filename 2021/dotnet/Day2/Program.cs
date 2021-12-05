var lines = File.ReadLines("../../inputs/day2.txt").ToList();
var instructions = lines.Select(x =>
{
   var values = x.Split(" ");
   return new { Direction = values[0], Distance = int.Parse(values[1]) };
}).ToList();

// First
var horizontalPos = 0;
var depth = 0;
foreach (var instruction in instructions)
{
   switch (instruction.Direction)
   {
      case "forward":
         horizontalPos += instruction.Distance;
         break;
      case "up":
         depth -= instruction.Distance;
         break;
      case "down":
         depth += instruction.Distance;
         break;
   }
}

Console.WriteLine(horizontalPos * depth); // 1604850

// Second

var aim = 0;
horizontalPos = 0;
depth = 0;
foreach (var instruction in instructions)
{
   switch (instruction.Direction)
   {
      case "forward":
         horizontalPos += instruction.Distance;
         depth += instruction.Distance * aim;
         break;
      case "up":
         aim -= instruction.Distance;
         break;
      case "down":
         aim += instruction.Distance;
         break;
   }
}

Console.WriteLine(horizontalPos * depth); // 1685186100 
