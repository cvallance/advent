using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    public class Program
    {
        static void Main(string[] args)
        {
            var groups = CreateGroups();
            var first = FirstSolution(groups);
            var second = SecondSolution(groups);
            
            Console.WriteLine($"Day 6 - Part 1: {first}");
            Console.WriteLine($"Day 6 - Part 2: {second}");
        }
        
        private static IEnumerable<IEnumerable<char[]>> CreateGroups()
        {
            var groupsData = File.ReadAllText("../../inputs/day6.txt");
            return ParseGroups(groupsData);
        }
        
        public static IEnumerable<IEnumerable<char[]>> ParseGroups(string allGroupData)
        {
            return allGroupData.Trim().Split("\n\n").Select(ParseGroup);
        }

        public static IEnumerable<char[]> ParseGroup(string groupData)
        {
            return groupData.Split("\n").Select(personData => personData.Trim().ToCharArray());
        }

        public static int FirstSolution(IEnumerable<IEnumerable<char[]>> groups)
        {
            return groups.Sum(FirstGroupCount);
        }

        public static int FirstGroupCount(IEnumerable<char[]> group)
        {
            var answers = new HashSet<char>();
            foreach (var c in group.SelectMany(person => person))
            {
                answers.Add(c);
            }

            return answers.Count;
        }

        public static int SecondSolution(IEnumerable<IEnumerable<char[]>> groups)
        {
            return groups.Sum(SecondGroupCount);
        }

        public static int SecondGroupCount(IEnumerable<char[]> group)
        {
            var first = true;
            var oldSet = new HashSet<char>();
            foreach (var person in group)
            {
                var newSet = new HashSet<char>();
                foreach (var c in person)
                {
                    if (first || oldSet.Contains(c))
                    {
                        newSet.Add(c);
                    }
                }

                first = false;
                oldSet = newSet;
            }

            return oldSet.Count;
        }

    }
}
