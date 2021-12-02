using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Shared;

namespace Day13
{
    public class Program
    {
        static void Main(string[] args)
        {
            var instructions = GetInstructions();
            var time = instructions.Item1;
            var buses = instructions.Item2.ToList();
            var first = FirstSolution(time, buses);
            Console.WriteLine($"Day 12 - Part 1: {first}");
            // var second = SecondSolution(buses);
            // Console.WriteLine($"Day 12 - Part 2: {second}");
        }

        private static int FirstSolution(int time, IEnumerable<int> buses)
        {
            var lowest = decimal.MaxValue;
            var lowestBus = 0;
            foreach (var bus in buses)
            {
                if (bus == 0) continue;

                var minsAfter = Math.Ceiling((decimal)time / bus) * bus - time;
                if (minsAfter < lowest)
                {
                    lowest = minsAfter;
                    lowestBus = bus;
                }
            }

            return (int)lowest * lowestBus;
        }
        
        public static long SecondSolution(IList<int> buses)
        {
            // Base Number
            long testNumber = buses.First(x => x != 0);
            // Increase Number
            var increaseNumber = testNumber;
            // Bus index
            var busIndex = 1;
            while (busIndex < buses.Count)
            {
                var busToTest = buses[busIndex];
                if ((testNumber + busIndex) % busToTest == 0)
                {
                    increaseNumber = FindLowestCommonMultiple(increaseNumber, buses[busIndex]);
                    busIndex += 1;
                }
                
                // If it isn't a match
                testNumber += increaseNumber;
            }

            return testNumber;
        }

        private static long FindLowestCommonMultiple(long first, int second)
        {
            long num1, num2;
            if (first > second)
            {
                num1 = first;
                num2 = second;
            }
            else
            {
                num1 = second;
                num2 = first;
            }

            for (long i = 1; i < num2; i++)
            {
                var mult = num1 * i;
                if (mult % num2 == 0)
                {
                    return mult;
                }
            }

            return num1 * num2;
        }

        public static long SecondSolutionOld(IList<int> buses)
        {
            var items = new Dictionary<int, int>();
            var biggest = 0;
            foreach (var (bus, index) in buses.WithIndex())
            {
                if (bus == 0) continue;
                items.Add(bus, index);
                if (bus > biggest) biggest = bus;
            }

            var multiplier = 1;
            while (true)
            {
                var found = true;
                var baseNumber = (long)biggest * multiplier - items[biggest];
                foreach (var (bus, index) in buses.WithIndex())
                {
                    if (bus == 0) continue;
                    if ((baseNumber + index) % bus == 0) continue;
                    
                    found = false;
                    break;
                }

                if (found)
                {
                    return baseNumber;
                }

                multiplier += 1;
            }
        }
        
        private static (int, IEnumerable<int>) GetInstructions()
        {
            return ParseInstructions(File.ReadAllText("../../inputs/day13.txt"));
        }
        
        public static (int, IEnumerable<int>) ParseInstructions(string instructions)
        {
            var lines = instructions.Trim().Split("\n");
            var time = int.Parse(lines[0]);
            var buses = lines[1].Split(",").Select(x => x == "x" ? 0 : int.Parse(x));
            return (time, buses);
        }
    }
}
