using System;
using System.IO;
using System.Linq;

namespace Day02
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("../../inputs/day2.txt").ToList();
            var firstResult = 0;
            var secondResult = 0;
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var times = parts[0].Split('-');
                var min = int.Parse(times[0]);
                var max = int.Parse(times[1]);
                
                var letter = char.Parse(parts[1].TrimEnd(':'));
                var passwordChars = parts[2].ToCharArray();
                var count = passwordChars.Count(x => x == letter);
                if (count >= min && count <= max)
                {
                    firstResult += 1;
                }

                var charAtMin = passwordChars[min - 1];
                var charAtMax = passwordChars[max - 1];
                if (charAtMin == letter && charAtMax != letter)
                {
                    secondResult += 1;
                }
                else if (charAtMin != letter && charAtMax == letter)
                {
                    secondResult += 1;
                }
            }
            
            Console.WriteLine($"Day 1 - Part 1: {firstResult}");
            Console.WriteLine($"Day 1 - Part 2: {secondResult}");
        }
    }
}
