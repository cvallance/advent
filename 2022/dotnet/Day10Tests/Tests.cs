using Day10;

namespace Day10Tests;

public class Tests
{
    private const string TestInput = @"addx 1
noop
addx 2
addx 5
addx 3
noop
noop
addx 4
noop
noop
noop
noop
noop
addx 6
noop
noop
noop
addx 4
addx 1
noop
addx 3
addx 2
noop
noop
addx 5
noop
noop
addx -38
addx 7
noop
noop
addx 3
noop
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx 6
noop
noop
noop
noop
addx 4
noop
noop
addx 5
noop
noop
noop
addx -34
noop
noop
noop
noop
noop
addx 4
noop
noop
noop
addx 1
addx 4
addx 1
addx 4
noop
noop
addx 6
noop
noop
noop
addx 5
noop
noop
addx 4
noop
noop
noop
addx 1
addx 5
noop
noop
addx -33
noop
noop
noop
noop
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
noop
addx 8
noop
noop
noop
noop
noop
addx 3
noop
addx 4
noop
noop
addx 8
noop
noop
noop
noop
noop
noop
addx -35
noop
noop
noop
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
addx 2
addx 6
noop
noop
noop
noop
addx 3
noop
addx 4
noop
noop
addx 8
noop
noop
noop
noop
noop
noop
addx -38
noop
noop
addx 5
noop
addx 3
noop
addx 2
addx 3
noop
addx 2
addx 3
noop
addx 2
noop
noop
addx 5
noop
noop
noop
addx 1
addx 4
addx 1
noop
addx 4
noop
noop
noop";
    
    [Fact]
    public void TestPart1()
    {
        // Arrange
        var expected = 16080;

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