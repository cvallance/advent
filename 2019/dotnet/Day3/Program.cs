using System;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("../../day3_input.txt").ToArray();
            var wires = ParseWires(lines);

            wires.firstWire.ComputePath();
            wires.secondWire.ComputePath();
            
            PartOne(wires.firstWire, wires.secondWire);
            PartTwo(wires.firstWire, wires.secondWire);
        }

        private static void PartOne(Wire firstWire, Wire secondWire)
        {
            var shortest = int.MaxValue;
            foreach (var point in firstWire.PathWithSteps.Keys)
            {
                if (secondWire.PathWithSteps.ContainsKey(point))
                {
                    var distance = Math.Abs(point.x) + Math.Abs(point.y);
                    if (distance < shortest)
                    {
                        shortest = distance;
                    }
                }
            }
            
            
            Console.WriteLine($"part 1 {shortest}");
        }
        
        private static void PartTwo(Wire firstWire, Wire secondWire)
        {
            var shortest = int.MaxValue;
            foreach (var point in firstWire.PathWithSteps.Keys)
            {
                if (secondWire.PathWithSteps.ContainsKey(point))
                {
                    var steps = firstWire.PathWithSteps[point] + secondWire.PathWithSteps[point];
                    if (steps < shortest)
                    {
                        shortest = steps;
                    }
                }
            }
            
            Console.WriteLine($"part 2 {shortest}");
        }
        
        private static (Wire firstWire, Wire secondWire) ParseWires(string[] lines)
        {
            // Regex would be better if I knew it
            var firstWire = ParseWire(lines[0]);
            var secondWire = ParseWire(lines[1]);

            return (firstWire, secondWire);
        }
        
        private static Wire ParseWire(string line)
        {
            // Regex would be better if I knew it
            var wire = new Wire();
            var rawInstructions = line.Split(",");
            foreach (var rawInstruction in rawInstructions)
            {
                wire.Instructions.Add(new Instruction
                {
                    Direction = rawInstruction.Substring(0, 1),
                    Distance = int.Parse(rawInstruction.Substring(1))
                });
            }

            return wire;
        }

    }
}
