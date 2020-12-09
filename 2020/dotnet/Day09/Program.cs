using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Day09
{
    public class Program
    {
        static void Main(string[] args)
        {
            var numbers = GetNumbers();
            var first = FindErroneousNumber(numbers, 25);
            
            Console.WriteLine($"Day 8 - Part 1: {first}");
            // Console.WriteLine($"Day 8 - Part 2: {second}");
        }
        
        private static IList<long> GetNumbers()
        {
            return File.ReadAllLines("../../inputs/day9.txt").Select(long.Parse).ToList();
        }

        public static long FindErroneousNumber(IList<long> numbers, int searchRange)
        {
            for (var position = searchRange; position < numbers.Count; position++)
            {
                var found = false;
                var value = numbers[position];
                for (var i = position - searchRange; i < position - 2; i++)
                {
                    for (var j = i + 1; j < position - 1; j++)
                    {
                        // ReSharper disable once InvertIf
                        var num1 = numbers[i];
                        var num2 = numbers[j];
                        var sum = num1 + num2;
                        if (sum == value)
                        {
                            found = true;
                            break;
                        }
                        
                        Console.WriteLine($"Checking {i} and {j} for {position} - {num1} + {num2} = {sum} != {value}");
                    }

                    if (found)
                    {
                        break;
                    }
                }

                if (!found)
                {
                    return value;
                }
            }

            return 0;
        }
    }
}
