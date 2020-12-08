using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        static void Main(string[] args)
        {
            var bags = CreateBags();
            foreach (var bag in bags)
            {
                Console.WriteLine(bag.Colour);
                foreach (var kvp in bag.ChildBags)
                {
                    Console.WriteLine($" - {kvp.Value} {kvp.Key}");
                }
            }
        }
        
        private static IEnumerable<Bag> CreateBags()
        {
            var bagsData = File.ReadAllLines("../../inputs/day7.txt");
            return ParseBags(bagsData);
        }

        private static IEnumerable<Bag> ParseBags(IEnumerable<string> bagsData)
        {
            var bags = new Dictionary<string, Bag>();
            foreach (var bagData in bagsData)
            {
                var bagsContain = "bags contain";
                var indexOfBagsContain = bagData.IndexOf(bagsContain, StringComparison.Ordinal);
                var bagColour = bagData.Substring(0, indexOfBagsContain - 1);
                var bag = new Bag(bagColour);
                var bagContents = bagData.Substring(indexOfBagsContain + bagsContain.Length + 1);
                if (bagContents == "no other bags.")
                {
                    return bag;
                }

                var childBagsData = bagContents.Split(", ");
                foreach (var childBagData in childBagsData)
                {
                    var hackySplit = childBagData.Split(' ');
                    var bagCount = int.Parse(hackySplit[0]);
                    var bagName = $"{hackySplit[1]} {hackySplit[2]}";
                    bag.ChildBags.Add(bagName, bagCount);
                }
                
                return bag;
            }

            return bags;
        }
    }

    public class Bag
    {
        public string Colour { get; }
        public IDictionary<string, int> ChildBags { get; }

        public Bag(string colour)
        {
            Colour = colour;
            ChildBags = new Dictionary<string, int>();
        }
    }
}
