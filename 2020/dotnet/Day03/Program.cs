using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = CreateMap();
            var first = CountTrees(map, 1, 1);
            var second = CountTrees(map, 3, 1);
            var third = CountTrees(map, 5, 1);
            var fourth = CountTrees(map, 7, 1);
            var fifth = CountTrees(map, 1, 2);
            
            Console.WriteLine($"Day 3 - Part 1: {second}");
            Console.WriteLine($"Day 3 - Part 2: {(long)first * second * third * fourth * fifth}");
        }

        private static int CountTrees(List<List<bool>> map, int right, int down)
        {
            var maxX = map[0].Count;
            var maxY = map.Count;
            
            var treeCount = 0;
            var x = 0;
            var y = 0;
            while (true)
            {
                x = (x + right) % maxX;
                y += down;
                if (y >= maxY)
                {
                    break;
                }
                
                if (map[y][x])
                {
                    treeCount += 1;
                }
            }

            return treeCount;
        }

        private static List<List<bool>> CreateMap()
        {
            var map = new List<List<bool>>();
            var lines = File.ReadLines("../../inputs/day3.txt").ToList();
            foreach (var line in lines)
            {
                var mapLine = new List<bool>();
                foreach (var point in line.ToCharArray())
                {
                    mapLine.Add(point == '#');
                }

                map.Add(mapLine);
            }

            return map;
        }
    }
}
