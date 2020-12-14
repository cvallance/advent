using System.Collections.Generic;
using System.Linq;
using Day12;
using Xunit;

namespace UnitTests
{
    public class Day12Tests
    {
        private List<int> ParseAdapters(string input)
        {
            var adapters = input.Split("\n").Select(int.Parse).OrderBy(x => x).ToList();
            return adapters.Concat(new[] {0, adapters[^1] + 3}).OrderBy(x => x).ToList();
        }
        
        [Fact]
        public void Part1()
        {
            var instructions = Program.ParseInstructions(@"F10
N3
F7
R90
F11");

            var result = Program.FirstSolution(instructions);
            Assert.Equal(25, result);
        }
        
        [Fact]
        public void Part2()
        {
            var instructions = Program.ParseInstructions(@"F10
N3
F7
R90
F11");

            var result = Program.SecondSolution(instructions);
            Assert.Equal(286, result);
        }
    }
}
