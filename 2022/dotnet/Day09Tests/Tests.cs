using Day09;

namespace Day09Tests;

public class Tests
{
    private const string TestInput = @"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2";

    private const string TestInputTwo = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
    
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var expected = 13;

        // Act
        var (part1, _) = Solver.Solve(TestInput);

        // Assert
        Assert.Equal(expected, part1);
    }
    
    [Fact]
    public void TestPart2()
    {
        // Arrange
        var expected = 36;
        
        // Act
        // var (_, part2) = Solver.Solve(TestInput);
        var (_, part2) = Solver.Solve(TestInputTwo);

        // Assert
        Assert.Equal(expected, part2);
    }
}