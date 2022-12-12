using Day12;

namespace Day12Tests;

public class Tests
{
    private const string TestInput = @"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";
    
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var expected = 31;

        // Act
        var (part1, _) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part1);
    }
    
    [Fact]
    public void TestPart2()
    {
        // Arrange
        var expected = 29;
        
        // Act
        var (_, part2) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part2);
    }
}