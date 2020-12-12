using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    public class Program
    {
        static void Main(string[] args)
        {
            var adapters = GetAdapters();
            var counts = new Dictionary<int, int> {{0, 0}, {1, 0}, {2, 0}, {3, 0}};
            var last = 0;
            foreach (var adapter in adapters)
            {
                var difference = adapter - last;
                counts[difference] = counts[difference] + 1;
                last = adapter;
            }

            var first = counts[1] * counts[3];
            Console.WriteLine($"Day 8 - Part 1: {first}");
            var second = GetPermutations(adapters);
            Console.WriteLine($"Day 8 - Part 2: {second}");
        }
        
        private static IList<int> GetAdapters()
        {
            var adapters = File.ReadAllLines("../../inputs/day10.txt").Select(int.Parse).OrderBy(x => x).ToList();
            return adapters.Concat(new[] {0, adapters[^1] + 3}).OrderBy(x => x).ToList();
        }

        public static long GetPermutations(IList<int> adapters)
        {
            var permutations = 1L;
            for (var i = 0; i < adapters.Count; i++)
            {
                var value = adapters[i];
                var inRange = 0;
                for (var j = i + 1; j <= i + 3 && j < adapters.Count; j++)
                {
                    Console.WriteLine($"Value:{value} This:{adapters[j]}");
                    if (adapters[j] > value + 3)
                    {
                        break;
                    }
                    
                    inRange += 1;
                }

                switch (inRange)
                {
                    case 2:
                        permutations += 1;
                        break;
                    case 3:
                        permutations += 2;
                        break;
                }
                
                Console.WriteLine($"Value:{value} InRange:{inRange} Multiplier:{permutations}");
            }

            return permutations * 2;
        }
        
        public static long GetPermutations_Old(IList<int> adapters)
        {
            var multipliers = new List<int> {1};
            for (var i = 0; i < adapters.Count; i++)
            {
                var value = adapters[i];
                var inRange = 0;
                for (var j = i + 1; j <= i + 3 && j < adapters.Count; j++)
                {
                    if (adapters[j] > value + 3)
                    {
                        break;
                    }
                    
                    inRange += 1;
                }

                var multiplier = 1;
                switch (inRange)
                {
                    case 2:
                        multiplier = 2;
                        i += 1;
                        break;
                    case 3:
                        multiplier = 4;
                        i += 1;
                        break;
                }
                permutations = permutations * multiplier;
                
                Console.WriteLine($"Value:{value} InRange:{inRange} Multiplier:{multiplier}");
            }

            return permutations;
        }
    }
}
