using Microsoft.VisualBasic;
using Shared;

// var lines = File.ReadLines("../../inputs/day15-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day15.txt").ToList();

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

var finish = new Vector(grid.Keys.Max(x => x.X), grid.Keys.Max(x => x.Y));

var dist = new Dictionary<Vector, int>();
var queue = new PriorityQueue<Vector, int>();
foreach (var item in grid)
{
    if (item.Key == new Vector(0, 0)) dist[item.Key] = 0;
    else dist[item.Key] = int.MaxValue;
}

queue.Enqueue(new Vector(0,0), 0);
while (queue.TryDequeue(out var item, out _))
{
    foreach (var adj in item.GetAdjacent(false))
    {
        if (!grid.ContainsKey(adj)) continue;
        
        var distToAdj = dist[item] + grid[adj];
        if (distToAdj < dist[adj])
        {
            dist[adj] = distToAdj;
            queue.Enqueue(adj, distToAdj);
        }
    }
}

Console.WriteLine(dist[finish]);