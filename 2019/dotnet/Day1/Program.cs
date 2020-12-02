using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("../../day1_input.txt").Select(int.Parse);
            PartOne(lines);
            PartTwo(lines);
        }

        public static void PartOne(IEnumerable<int> modules)
        {
            var total = 0;
            foreach (var moduleSize in modules)
            {
                total += CalcFuel(moduleSize);
            }
            Console.WriteLine($"Part 1: {total}");
        }

        public static void PartTwo(IEnumerable<int> modules)
        {
            var total = 0;
            foreach (var moduleSize in modules)
            {
                total += CalcFuelRecursize(moduleSize);
            }
            Console.WriteLine($"Part 2: {total}");
        }

        private static int CalcFuel(int moduleSize)
        {
            return moduleSize / 3 - 2;
        }

        private static int CalcFuelRecursize(int moduleSize)
        {
            var total = 0;
            while (moduleSize > 8)
            {
                var fuel = CalcFuel(moduleSize);
                total += fuel;
                moduleSize = fuel;
            }
            return total;
        }
    }
}
