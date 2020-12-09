using System.Linq;
using Day09;
using Xunit;

namespace UnitTests
{
    public class Day09Tests
    {
        [Fact]
        public void Part2()
        {
            var numbers = @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576".Split("\n").Select(long.Parse).ToList();

            var result = Program.FindErroneousNumber(numbers, 5);
            Assert.Equal(127, result);
        }
    }
}
