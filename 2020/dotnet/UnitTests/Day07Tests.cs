using Day07;
using Xunit;

namespace UnitTests
{
    public class Day07Tests
    {
        public void Part2()
        {
            var bagsData = @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";
            
            var bags = Program.ParseBags(bagsData.Split("\n"));
            var result = 0;
            foreach (var bag in bags)
            {
                if (bag.Colour == "shiny gold")
                {
                    result = bag.SubBagCount();
                }
            }
            
            Assert.Equal(126, result);
        }
    }
}
