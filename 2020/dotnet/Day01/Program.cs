using System;
using System.IO;
using System.Linq;

namespace Day01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var firstFound = false;
            var secondFound = false;
            var firstResult = 0;
            var secondResult = 0;
            var lines = File.ReadLines("../../inputs/day1.txt").ToList();
            for (var i = 0; i < lines.Count; i++)
            {
                var firstValue = int.Parse(lines[i]);
                for (var j = i + 1; j < lines.Count; j++)
                {
                    var secondValue = int.Parse(lines[j]);
                    if (firstValue + secondValue == 2020)
                    {
                        firstResult = firstValue * secondValue;
                        firstFound = true;
                        break;
                    }
                    
                    for (var k = j + 1; k < lines.Count; k++)
                    {
                        var thirdValue = int.Parse(lines[k]);
                        if (firstValue + secondValue + thirdValue == 2020)
                        {
                            secondResult = firstValue * secondValue * thirdValue;
                            secondFound = true;
                            break;
                        }
                    }
                }
                
                if (firstFound && secondFound)
                {
                    break;
                }
            }
            
            Console.WriteLine($"Day 1 - Part 1: {firstResult}");
            Console.WriteLine($"Day 1 - Part 2: {secondResult}");
        }
    }
}
