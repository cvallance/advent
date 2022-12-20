using System.Diagnostics;
using System.Drawing;
using System.Text;
using Shared;

namespace Day17;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var sw = Stopwatch.StartNew();
        Console.WriteLine($"Parsing {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        var part1 = Part1(new Queue<char>(data));
        Console.WriteLine($"Part1 {sw.ElapsedMilliseconds}ms");
        
        sw.Restart();
        var part2 = Part2(new Queue<char>(data));
        Console.WriteLine($"Part2 {sw.ElapsedMilliseconds}ms");
        
        return (part1, part2);
    }

    private static int Part1(Queue<char> movements)
    {
        var (pieces, highest) = SimulateTetris(movements, 2022);
        return highest;
    }

    private static int Part2(Queue<char> movements)
    {
        var (pieces, highest) = SimulateTetris(movements, movements.Count * 5);
        PrintPieces(pieces, true);
        return highest;
    }

    private static (HashSet<Vector>, int) SimulateTetris(Queue<char> movements, int numberOfRocks)
    {
        var pieces = new HashSet<Vector>
        {
            // Start with the floor
            new(0, 0), new(1, 0), new(2, 0), new(3, 0), new(4, 0), new(5, 0), new(6, 0)
        };

        var highestPiece = 0;
        for (var i = 1; i <= numberOfRocks; i++)
        {
            var shape = ModToShape[i % 5]();
            // I feel that +4 should be +3 given the instructions, but imagery tells me otherwise
            shape = shape.Move(new Vector(2, highestPiece + 4));

            var isJet = false;
            while (true)
            {
                isJet = !isJet;
                if (isJet)
                {
                    var movement = movements.Dequeue();
                    movements.Enqueue(movement);
                    var movementVec = movement switch
                    {
                        '>' => new Vector(1, 0),
                        '<' => new Vector(-1, 0)
                    };
                    var newShape = shape.Move(movementVec);
                    if (newShape.IsOutsideBounds()) continue;
                    if (newShape.IsOverlapping(pieces)) continue;

                    // If it's not outside the bounds and it's not overlapping, the newShape is now the shape
                    shape = newShape;
                }
                else
                {
                    var movementVec = new Vector(0, -1);
                    var newShape = shape.Move(movementVec);
                    if (newShape.IsOverlapping(pieces))
                    {
                        shape.Solidify(pieces);
                        var highestPoint = shape.HighestPoint();
                        if (highestPoint > highestPiece) highestPiece = highestPoint;

                        // PrintPieces(pieces);
                        // Thread.Sleep(500);
                        break;
                    }

                    shape = newShape;
                }
            }
        }

        return (pieces, highestPiece);
    }

    private static readonly Dictionary<int, Func<Shape>> ModToShape = new()
    {
        { 1, () => new HLine() },
        { 2, () => new Plus() },
        { 3, () => new Corner() },
        { 4, () => new VLine() },
        { 0, () => new Square() }
    };
    
    private class Shape
    {
        protected IList<Vector> Points;

        protected Shape(params Vector[] points)
        {
            Points = new List<Vector>(points);
        }

        public Shape Move(Vector movement)
        {
            return new Shape(Points.Select(x => x + movement).ToArray());
        }

        public bool IsOutsideBounds()
        {
            return Points.Any(x => x.X is < 0 or > 6);
        }

        public bool IsOverlapping(HashSet<Vector> pieces)
        {
            return Points.Any(pieces.Contains);
        }

        public void Solidify(HashSet<Vector> pieces)
        {
            foreach (var point in Points)
            {
                pieces.Add(point);
            }
        }

        public int HighestPoint()
        {
            return Points.Max(x => x.Y);
        }
    }
    
    private class HLine : Shape
    {
        public HLine() : base(
            new Vector(0, 0),
            new Vector(1, 0),
            new Vector(2, 0),
            new Vector(3, 0)
        )
        {
        }
    }

    private class Plus : Shape
    {
        public Plus() : base(
            new Vector(1, 0),
            new Vector(0, 1),
            new Vector(1, 1),
            new Vector(2, 1),
            new Vector(1, 2)
        )
        {
        }
    }

    private class Corner : Shape
    {
        public Corner() : base(
            new Vector(0, 0),
            new Vector(1, 0),
            new Vector(2, 0),
            new Vector(2, 1),
            new Vector(2, 2)
        )
        {
        }
    }

    private class VLine : Shape
    {
        public VLine() : base(
            new Vector(0, 0),
            new Vector(0, 1),
            new Vector(0, 2),
            new Vector(0, 3)
        )
        {
        }
    }
    
    private class Square : Shape
    {
        public Square() : base(
            new Vector(0, 0),
            new Vector(1, 0),
            new Vector(0, 1),
            new Vector(1, 1)
        )
        {
        }
    }

    private static void PrintPieces(HashSet<Vector> vectors, bool head = false)
    {
        Console.WriteLine();
        var maxY = vectors.Max(x => x.Y);
        var minY = head ? maxY - 5 : 0;
        for (var y = maxY; y >= minY; y--)
        {
            var line = new StringBuilder("|");
            for (var x = 0; x < 7; x++)
            {
                line.Append(vectors.Contains(new Vector(x, y)) ? '#' : '.');
            }

            line.Append("|");
            Console.WriteLine(line);
        }
        Console.WriteLine();
    }
}