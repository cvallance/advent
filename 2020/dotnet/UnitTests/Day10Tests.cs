using System.Collections.Generic;
using System.Linq;
using Day10;
using Xunit;

namespace UnitTests
{
    public class Day10Tests
    {
        [Fact]
        public void Part2_Easy()
        {
            var adapters = @"16
10
15
5
1
11
7
19
6
12
4".Split("\n").Select(int.Parse).OrderBy(x => x).ToList();

            var result = Program.GetPermutations(adapters);
            Assert.Equal(8, result);
        }
        
        [Fact]
        public void Part2_Easy_Ordered()
        {
            var adapters = new List<int>
            {
                1,  // 1  1
                4,  // 3  3
                5,  // 2  
                6,  // 1  
                7,  // 1 
                10, // 2 
                11, // 1
                12, // 1
                15, // 1
                16, // 1
                19  // 1
            };
            
            /*
             * 1
             * 1 4
             * 1 4 5
             * 
             */

            var adapters = new List<int>
            {
                1,  // 1 1 3 5 5
                4,  // 3 1 3 5 5
                5,  // 2 1 1 2 2
                6,  // 1 1 1 2 2
                7,  // 1 1 1 2 2
                10, // 2 1 3 4 5
                11, // 1 1 3 4 5
                12, // 1 1 3 4 5
                15, // 1 1 3 4 5
                16, // 1 1 3 4 5
                19  // 1 1 3 4 5
            };

            var result = Program.GetPermutations(adapters);
            Assert.Equal(8, result);
        }
        
        [Fact(Skip = "Leave fore now")]
        public void Part2_Medium()
        {
            var adapters = @"28
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
3".Split("\n").Select(int.Parse).OrderBy(x => x).ToList();

            var result = Program.GetPermutations(adapters);
            Assert.Equal(19208, result);
        }
    }
}
