using System.Collections.Generic;
using System.Linq;
using Day10;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Xunit;

namespace UnitTests
{
    public class Day10Tests
    {
        private List<int> ParseAdapters(string input)
        {
            var adapters = input.Split("\n").Select(int.Parse).OrderBy(x => x).ToList();
            return adapters.Concat(new[] {0, adapters[^1] + 3}).OrderBy(x => x).ToList();
        }
        
        [Fact]
        public void Part2_Easy()
        {
            var adapters = ParseAdapters(@"16
10
15
5
1
11
7
19
6
12
4");

            var result = Program.GetPermutations(adapters);
            Assert.Equal(8, result);
        }
        
        [Fact]
        public void Part2_Medium()
        {
            var adapters = ParseAdapters(@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3");

            var result = Program.GetPermutations(adapters);
            Assert.Equal(19208, result);
        }
    }
}
