using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Day07
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bags = CreateBags();
            var first = 0;
            var second = 0;
            foreach (var bag in bags)
            {
                if (bag.HasChildBag("shiny gold"))
                {
                    first += 1;
                }

                if (bag.Colour == "shiny gold")
                {
                    second = bag.SubBagCount();
                }
            }
            
            Console.WriteLine($"Day 7 - Part 1: {first}");
            Console.WriteLine($"Day 7 - Part 2: {second}");
        }
        
        private static IEnumerable<Bag> CreateBags()
        {
            var bagsData = File.ReadAllLines("../../inputs/day7.txt");
            return ParseBags(bagsData);
        }

        public static IEnumerable<Bag> ParseBags(IEnumerable<string> bagsData)
        {
            var bags = new Dictionary<string, Bag>();
            foreach (var bagData in bagsData)
            {
                var bagsContain = "bags contain";
                var indexOfBagsContain = bagData.IndexOf(bagsContain, StringComparison.Ordinal);
                var bagColour = bagData.Substring(0, indexOfBagsContain - 1);
                if (!bags.TryGetValue(bagColour, out var bag))
                {
                    bag = new Bag(bagColour);
                    bags.Add(bag.Colour, bag);
                }

                var bagContents = bagData.Substring(indexOfBagsContain + bagsContain.Length + 1);
                if (bagContents == "no other bags.")
                {
                    continue;
                }

                var childBagsData = bagContents.Split(", ");
                foreach (var childBagData in childBagsData)
                {
                    var hackySplit = childBagData.Split(' ');
                    var childBagCount = int.Parse(hackySplit[0]);
                    var childBagColour = $"{hackySplit[1]} {hackySplit[2]}";
                    if (!bags.TryGetValue(childBagColour, out var childBag))
                    {
                        childBag = new Bag(childBagColour);
                        bags.Add(childBagColour, childBag);
                    }
                    bag.ChildBags.Add(childBag, childBagCount);
                }
            }

            return bags.Values;
        }
    }

    public class Bag
    {
        public string Colour { get; }
        public IDictionary<Bag, int> ChildBags { get; }

        public Bag(string colour)
        {
            Colour = colour;
            ChildBags = new Dictionary<Bag, int>();
        }

        public bool HasChildBag(string childColourToFind)
        {
            foreach (var (childBag, childBagCount) in ChildBags)
            {
                if (childBag.Colour == childColourToFind || childBag.HasChildBag(childColourToFind))
                {
                    return true;
                }
            }

            return false;
        }
        
        public int SubBagCount()
        {
            var count = 0;
            foreach (var (childBag, childBagCount) in ChildBags)
            {
                count += childBagCount;
                count += childBagCount * childBag.SubBagCount();
            }

            return count;
        }

        public override int GetHashCode()
        {
            return Colour.GetHashCode();
        }
    }
}
