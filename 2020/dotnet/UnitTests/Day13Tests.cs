using System.Collections.Generic;
using System.Linq;
using Day13;
using Xunit;

namespace UnitTests
{
    public class Day13Tests
    {
        private static IList<int> ParseBuses(string buses)
        {
            return buses.Split(",").Select(x => x == "x" ? 0 : int.Parse(x)).ToList();
        }

        [Theory]
        [InlineData("17,x,13,19", 3417)]
        [InlineData("67,7,59,61", 754018)]
        [InlineData("67,x,7,59,61", 779210)]
        [InlineData("67,7,x,59,61", 1261476)]
        [InlineData("1789,37,47,1889", 1202161486)]
        public void Part2_One(string busesString, long expectedResult)
        {
            var buses = ParseBuses(busesString);

            var result = Program.SecondSolution(buses);
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("67,7", 335)]
        [InlineData("67,7,59", 6901)]
        [InlineData("67,7,59,61", 754018)]
        public void Part2_Two(string busesString, long expectedResult)
        {
            var buses = ParseBuses(busesString);

            var result = Program.SecondSolution(buses);
            Assert.Equal(expectedResult, result);
        }
        
        [Theory]
        [InlineData("67,x,7", 201)]
        [InlineData("67,x,7,59", 4422)]
        [InlineData("67,x,7,59,61", 779210)]
        [InlineData("67,x,x,x,61", 2680)]
        public void Part2_Three(string busesString, long expectedResult)
        {
            var buses = ParseBuses(busesString);

            var result = Program.SecondSolution(buses);
            Assert.Equal(expectedResult, result);
        }
    }
}
