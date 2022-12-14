using Shared;

namespace Day14;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var cave = new HashSet<Vector>();

        foreach (var line in lines)
        {
            var rockLines = line.Split(" -> ");
            for (var i = 0; i < rockLines.Length - 1; i++)
            {
                var parts = rockLines[i].Split(',');
                var firstPoint = new Vector(int.Parse(parts[0]), int.Parse(parts[1]));
                parts = rockLines[i+1].Split(',');
                var secondPoint = new Vector(int.Parse(parts[0]), int.Parse(parts[1]));

                var movement = new Vector(0, 0);
                if (firstPoint.X == secondPoint.X)
                {
                    movement = firstPoint.Y < secondPoint.Y ? new Vector(0, 1) : new Vector(0, -1);
                }
                else
                {
                    movement = firstPoint.X < secondPoint.X ? new Vector(1, 0) : new Vector(-1, 0);
                }

                while (firstPoint != secondPoint)
                {
                    cave.Add(firstPoint);
                    firstPoint += movement;
                }

                cave.Add(secondPoint);
            }
        }

        var part2Cave = new HashSet<Vector>(cave);
        var part1 = Part1(cave);
        var part2 = Part2(part2Cave);
        
        return (part1, part2);
    }

    private static int Part1(HashSet<Vector> cave)
    {
        var down = new Vector(0, 1);
        var downLeft = new Vector(-1, 1);
        var downRight = new Vector(1, 1);
            
        var max = cave.Max(x => x.Y);
        var sandBits = 0;
        while (true)
        {
            var sandVector = new Vector(500, 0);
            while (true)
            {
                if (sandVector.Y > max) return sandBits;
                
                var test = sandVector + down;
                if (!cave.Contains(test))
                {
                    sandVector = test;
                    continue;
                }

                test = sandVector + downLeft;
                if (!cave.Contains(test))
                {
                    sandVector = test;
                    continue;
                }

                test = sandVector + downRight;
                if (!cave.Contains(test))
                {
                    sandVector = test;
                    continue;
                }

                cave.Add(sandVector);
                sandBits++;
                break;
            }
        }
    }

    private static int Part2(HashSet<Vector> cave)
    {
        var down = new Vector(0, 1);
        var downLeft = new Vector(-1, 1);
        var downRight = new Vector(1, 1);
            
        var max = cave.Max(x => x.Y) + 2;
        var sandBits = 0;
        while (true)
        {
            var sandVector = new Vector(500, 0);
            while (true)
            {
                var test = sandVector + down;
                if (!cave.Contains(test) && test.Y != max)
                {
                    sandVector = test;
                    continue;
                }

                test = sandVector + downLeft;
                if (!cave.Contains(test) && test.Y != max)
                {
                    sandVector = test;
                    continue;
                }

                test = sandVector + downRight;
                if (!cave.Contains(test) && test.Y != max)
                {
                    sandVector = test;
                    continue;
                }

                if (sandVector == new Vector(500, 0)) return ++sandBits;
                
                cave.Add(sandVector);
                sandBits++;
                break;
            }
        }
    }
}