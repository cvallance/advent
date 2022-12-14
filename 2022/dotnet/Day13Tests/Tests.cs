using Day13;

namespace Day13Tests;

public class Tests
{
    private const string TestInput = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
    
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var expected = 0;

        // Act
        var (part1, _) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part1);
    }
    
    [Fact]
    public void TestPart2()
    {
        // Arrange
        var expected = 0;
        
        // Act
        var (_, part2) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part2);
    }
}