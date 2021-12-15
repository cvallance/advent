using System.ComponentModel.Design;
using Shared;

// var lines = File.ReadLines("../../inputs/day13-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day13.txt").ToList();

var dots = new HashSet<Vector>();
var folds = new Queue<Fold>();
foreach (var line in lines)
{
    if (line.Contains(','))
    {
        var xy = line.Split(',');
        dots.Add(new Vector(int.Parse(xy[0]), int.Parse(xy[1])));
    }
    else if (line.StartsWith("fold along"))
    {
        var fold = line[11..].Split('=');
        folds.Enqueue(new Fold { Axis = fold[0], Value = int.Parse(fold[1])});
    }
}

while (folds.TryDequeue(out var fold))
{
    var newDots = new HashSet<Vector>();
    foreach (var dot in dots)
    {
        if ((fold.Axis == "x" && dot.X < fold.Value) || (fold.Axis == "y" && dot.Y < fold.Value))
        {
            newDots.Add(dot);
        }
        else if (fold.Axis == "x" && dot.X > fold.Value)
        {
            var newX = fold.Value - (dot.X - fold.Value);
            newDots.Add(new Vector(newX, dot.Y));
        }
        else if (fold.Axis == "y" && dot.Y > fold.Value)
        {
            var newY = fold.Value - (dot.Y - fold.Value);
            newDots.Add(new Vector(dot.X, newY));
        }
    }

    dots = newDots;
}

var maxY = dots.Max(x => x.Y);
var maxX = dots.Max(x => x.X);
for (var y = 0; y <= maxY; y++)
{
    for (var x = 0; x <= maxX; x++)
    {
        if (dots.Contains(new Vector(x, y)))
        {
            Console.Write("*");
        }
        else
        {
            Console.Write(" ");
        }
    }
    Console.WriteLine();
}

Console.WriteLine(dots.Count);