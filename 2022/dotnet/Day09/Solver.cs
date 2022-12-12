using Shared;

namespace Day09;


public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var part1 = Part1(lines);
        var part2 = Part2(lines);
        return (part1, part2);
    }

    private static int Part1(List<string> lines)
    {
        var head = new Vector(0, 0);
        var tail = new Vector(0, 0);

        var visited = new HashSet<Vector>(new[] { tail });

        foreach (var ins in lines)
        {
            var parts = ins.Split(" ");
            var directionVector = parts[0] switch
            {
                "U" => new Vector(0, 1),
                "D" => new Vector(0, -1),
                "L" => new Vector(-1, 0),
                "R" => new Vector(1, 0),
                _ => throw new Exception("Pants")
            };
            var distance = int.Parse(parts[1]);
            for (var i = 1; i <= distance; i++)
            {
                head += directionVector;
                // if the tail don't need to move, don't move
                if (head == tail || head.GetAdjacent(true).Any(x => x == tail)) continue;

                // If we're on the same row or column, we can just move in the same direction
                if (head.X == tail.X || head.Y == tail.Y) tail += directionVector;
                else
                {
                    if (Math.Abs(head.X - tail.X) == 1)
                    {
                        tail = new Vector(head.X, tail.Y) + directionVector;
                    }
                    else
                    {
                        tail = new Vector(tail.X, head.Y) + directionVector;
                    }
                }

                visited.Add(tail);
            }
        }

        return visited.Count;
    }

    private static int Part2(List<string> lines)
    {
        Console.WriteLine($"");
        Console.WriteLine($"Part 2");
        var ropeParts = new Dictionary<int, Vector>()
        {
            { 0, new Vector(0, 0) },
            { 1, new Vector(0, 0) },
            { 2, new Vector(0, 0) },
            { 3, new Vector(0, 0) },
            { 4, new Vector(0, 0) },
            { 5, new Vector(0, 0) },
            { 6, new Vector(0, 0) },
            { 7, new Vector(0, 0) },
            { 8, new Vector(0, 0) },
            { 9, new Vector(0, 0) }
        };

        var visited = new HashSet<Vector>(new[] { new Vector() });

        foreach (var ins in lines)
        {
            var parts = ins.Split(" ");
            var directionVector = parts[0] switch
            {
                "U" => new Vector(0, 1),
                "D" => new Vector(0, -1),
                "L" => new Vector(-1, 0),
                "R" => new Vector(1, 0),
                _ => throw new Exception("Pants")
            };
            var distance = int.Parse(parts[1]);

            for (var i = 1; i <= distance; i++)
            {
                Console.WriteLine($"Blah {ins}, {i}");
                
                // Move the head
                ropeParts[0] += directionVector;

                for (var j = 0; j < 9; j++)
                {
                    var leadingKnot = ropeParts[j];
                    Console.WriteLine($"Leading knot {j} -> {leadingKnot}");
                    var trailingKnot = ropeParts[j + 1];
                    Console.WriteLine($" - {j+1} before {trailingKnot}");
                    // if the tail don't need to move, don't move
                    if (leadingKnot == trailingKnot || leadingKnot.GetAdjacent(true).Any(x => x == trailingKnot)) continue;

                    // If we're on the same row or column, we can just move in the same direction
                        var xDirection = new Vector(0, 0);
                        var yDirection = new Vector(0, 0);
                        if (leadingKnot.X != trailingKnot.X)
                        {
                            xDirection = leadingKnot.X > trailingKnot.X
                                ? new Vector(1, 0)
                                : new Vector(-1, 0);
                        }
                        
                        if (leadingKnot.Y != trailingKnot.Y)
                        {
                            yDirection = leadingKnot.Y > trailingKnot.Y
                                ? new Vector(0, 1)
                                : new Vector(0, -1);
                        }

                        trailingKnot += xDirection + yDirection;

                    Console.WriteLine($" - {j+1} after  {trailingKnot}");
                    ropeParts[j + 1] = trailingKnot;
                }
                
                visited.Add(ropeParts[9]);
            }
        }

        return visited.Count;
    }
}