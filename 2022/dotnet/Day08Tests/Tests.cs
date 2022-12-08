using Day08;

namespace Day08Tests;

public class Tests
{
    private const string TestInput = @"30373
25512
65332
33549
35390";
    
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var expected = 21;

        // Act
        var (part1, _) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part1);
    }
    
    [Fact]
    public void TestPart2()
    {
        // Arrange
        var expected = 8;
        
        // Act
        var (_, part2) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part2);
    }
}