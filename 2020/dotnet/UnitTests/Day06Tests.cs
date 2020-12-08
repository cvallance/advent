using Day06;
using Xunit;

namespace UnitTests
{
    public class Day06Tests
    {
        [Theory]
        [InlineData("abc", 3)]
        [InlineData(@"a
b
c", 3)]
        [InlineData(@"ab
ac", 3)]
        [InlineData(@"a
a
a
a", 1)]
        [InlineData(@"b", 1)]
        public void Test1(string groupData, int firstSolution)
        {
            var group = Program.ParseGroup(groupData);
            var firstResult = Program.FirstGroupCount(group);
            Assert.Equal(firstSolution, firstResult);
        }
        
        [Theory]
        [InlineData(@"abc", 3)]
        [InlineData(@"a
b
c", 0)]
        [InlineData(@"ab
ac", 1)]
        [InlineData(@"abc
abc
abe
aef", 1)]
        [InlineData(@"abcdefg
abcdefg
abceg
abcde
abcdef", 4)]
        [InlineData(@"a
a
a
a", 1)]
        [InlineData(@"b", 1)]
        public void Test2(string groupData, int firstSolution)
        {
            var group = Program.ParseGroup(groupData);
            var secondResult = Program.SecondGroupCount(group);
            Assert.Equal(firstSolution, secondResult);
        }
        
        [Fact]
        public void Test2Full()
        {
            var groupsData = @"abc

a
b
c

ab
ac

a
a
a
a

b";
            var groups = Program.ParseGroups(groupsData);
            var secondResult = Program.SecondSolution(groups);
            Assert.Equal(6, secondResult);
        }
    }
}
