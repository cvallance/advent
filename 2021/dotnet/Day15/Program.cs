using Microsoft.VisualBasic;
using Shared;

var lines = File.ReadLines("../../inputs/day15-test.txt").ToList();
// var lines = File.ReadLines("../../inputs/day15.txt").ToList();

var grid = new Dictionary<Vector, int>();
for (var y = 0; y < lines.Count; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        var risk = int.Parse(lines[y][x].ToString());
        grid.Add(new Vector(x, y), risk);
    }
}

// Create bigger grid
var origWidth = grid.Keys.Max(x => x.X) + 1;
var origHeight = grid.Keys.Max(x => x.Y) + 1;
var origGrid = new Dictionary<Vector, int>(grid);
for (var y = 0; y < 5; y++)
{
    for (var x = 0; x < 5; x++)
    {
        if (x == 0 && y == 0) continue;

        foreach (var point in origGrid)
        {
            var newPoint = new Vector(
                origWidth * x + point.Key.X,
                origHeight * y + point.Key.Y
            );
            var newRisk = (point.Value + x + y) % 9;
            if (newRisk == 0) newRisk = 9;
            grid.Add(newPoint, newRisk);
        }
    }
}

var maxX = grid.Keys.Max(x => x.X);
var maxY = grid.Keys.Max(x => x.Y);
var finish = new Vector(maxX, maxY);

var lowestScore = int.MaxValue;
var startPoint = new Vector(0, 0);
var lowestScores = new Dictionary<Vector, int>();
void DepthScore(Vector point, int score, HashSet<Vector> visited, bool extended)
{
    if (score >= lowestScore) return;
    if (point == finish)
    {
        lowestScore = score;
        return;
    }

    // We should say points more than 4 left or 4 up are visited because even if they're all 1s, it would be more
    // expensive to visit them just to get back
    if (point.X < startPoint.X - 3) return;
    if (point.Y < startPoint.Y - 3) return;
    
    if (visited.Contains(point)) return;
    visited.Add(point);

    var pointsToTry = new Dictionary<Vector, char>();
    var down = new Vector(point.X, point.Y + 1);
    if (grid.ContainsKey(down)) pointsToTry.Add(down, 'd');
    var right = new Vector(point.X + 1, point.Y);
    if (grid.ContainsKey(right)) pointsToTry.Add(right, 'r');
    if (extended)
    {
        var left = new Vector(point.X - 1, point.Y);
        if (grid.ContainsKey(left)) pointsToTry.Add(left, 'l');
        var up = new Vector(point.X, point.Y - 1);
        if (grid.ContainsKey(up)) pointsToTry.Add(up, 'u');
    }

    foreach (var kvp in pointsToTry)
    {
        if (lowestScores.ContainsKey(kvp.Key))
        {
            var newScore = score + lowestScores[kvp.Key];
            if (newScore >= lowestScore) continue;
            lowestScore = newScore;
            continue;
        }

        var newVisited = new HashSet<Vector>(visited);
        if (kvp.Value is 'u' or 'd')
        {
            newVisited.Add(new Vector(point.X + 1, point.Y));
            newVisited.Add(new Vector(point.X - 1, point.Y));
        }
        else
        {
            newVisited.Add(new Vector(point.X, point.Y + 1));
            newVisited.Add(new Vector(point.X, point.Y - 1));
        }
        DepthScore(kvp.Key, score + grid[kvp.Key], newVisited, extended);
    }
}

for (var y = finish.Y; y >= 0; y--)
{
    for (var x = finish.X; x >= 0; x--)
    {
        lowestScore = int.MaxValue;
        startPoint = new Vector(x, y);
        var riskToStart = startPoint == new Vector(0, 0) ? 0 : grid[startPoint];
        DepthScore(startPoint, riskToStart, new HashSet<Vector>(), false);
        DepthScore(startPoint, riskToStart, new HashSet<Vector>(), true);
        Console.WriteLine($"{startPoint} => {lowestScore}");
        lowestScores.Add(startPoint, lowestScore);
    }
}


Console.WriteLine(lowestScore);