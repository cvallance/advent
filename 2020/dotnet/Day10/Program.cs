using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day10
{
    public class Program
    {
        static void Main(string[] args)
        {
            var adapters = GetAdapters();
            // Part 1 vars
            var counts = new Dictionary<int, int> {{0, 0}, {1, 0}, {2, 0}, {3, 0}};
            
            foreach (var (adapter, index) in adapters.WithIndex())
            {
                var difference = adapter - adapters[Math.Max(0, index - 1)];
                counts[difference] = counts[difference] + 1;
            }
            
            var first = counts[1] * counts[3];
            Console.WriteLine($"Day 10 - Part 1: {first}");
            var second = GetPermutations(adapters);
            Console.WriteLine($"Day 10 - Part 2: {second}");
        }
        
        private static IList<int> GetAdapters()
        {
            var adapters = File.ReadAllLines("../../inputs/day10.txt").Select(int.Parse).OrderBy(x => x).ToList();
            return adapters.Concat(new[] {0, adapters[^1] + 3}).OrderBy(x => x).ToList();
        }

        public static long GetPermutations(IList<int> adapters)
        {
            // Part 2 vars
            var branchCount = new Dictionary<int, long> {{0, 1}};
            foreach (var adapter in adapters.Skip(1))
            {
                var waysToBranch = branchCount.GetValueOrDefault(adapter - 1)
                                   + branchCount.GetValueOrDefault(adapter - 2)
                                   + branchCount.GetValueOrDefault(adapter - 3);
                branchCount[adapter] = waysToBranch;
            }

            return branchCount[adapters[^1]];
        }
    }
}
