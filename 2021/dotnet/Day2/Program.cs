// See https://aka.ms/new-console-template for more information

var lines = File.ReadLines("../../inputs/day2.txt").ToList();
var instructions = lines.Select(x =>
{
   var values = x.Split(" ");
   return new { Direction = values[0], Num = int.Parse(values[1]) };
}).ToList();

// First
var horizontalPos = 0;
var depth = 0;
foreach (var instruction in instructions)
{
   switch (instruction.Direction)
   {
      case "forward":
         horizontalPos += instruction.Num;
         break;
      case "up":
         depth -= instruction.Num;
         break;
      case "down":
         depth += instruction.Num;
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
         horizontalPos += instruction.Num;
         depth += instruction.Num * aim;
         break;
      case "up":
         aim -= instruction.Num;
         break;
      case "down":
         aim += instruction.Num;
         break;
   }
}

Console.WriteLine(horizontalPos * depth); // 1685186100 
