using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    public class Program
    {
        static void Main(string[] args)
        {
            var numbers = GetNumbers();
            var first = FindErroneousNumber(numbers, 25);
            var second = FindWeakness(numbers, first);
            
            Console.WriteLine($"Day 8 - Part 1: {first}");
            Console.WriteLine($"Day 8 - Part 2: {second}");
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
                for (var i = position - searchRange; i < position - 1; i++)
                {
                    for (var j = i + 1; j < position; j++)
                    {
                        // ReSharper disable once InvertIf
                        if (numbers[i] + numbers[j] == value)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found) break;
                }

                if (!found) return value;
            }

            return 0;
        }
        
        public static long FindWeakness(IList<long> numbers, long number)
        {
            for (var i = 0; i < numbers.Count; i++)
            {
                var smallest = numbers[i];
                var largest = numbers[i];
                
                var j = i + 1;
                var sum = numbers[i];
                while (sum < number)
                {
                    var loopValue = numbers[j];
                    if (loopValue < smallest) smallest = loopValue;
                    if (loopValue > largest) largest = loopValue;
                    
                    sum += loopValue;
                    if (sum == number) return smallest + largest;

                    j++;
                }
            }

            return 0;
        }
    }
}
