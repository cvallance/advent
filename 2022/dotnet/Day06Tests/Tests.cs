using Day06;

namespace Day06Tests;

public class Tests
{
    private const string TestInput = @"";
    
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