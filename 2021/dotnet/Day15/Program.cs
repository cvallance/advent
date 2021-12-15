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

var finish = new Vector(grid.Keys.Max(x => x.X), grid.Keys.Max(x => x.Y));

var lowestScore = int.MaxValue;
var lowestScores = new Dictionary<Vector, int>();
void DepthScore(Vector point, int score)
{
    if (score >= lowestScore) return;
    if (point == finish)
    {
        lowestScore = score;
        return;
    }
    
    // Lets assume we just go down and right
    var pointsToTry = new List<Vector>();
    var down = new Vector(point.X, point.Y + 1);
    if (grid.ContainsKey(down)) pointsToTry.Add(down);
    var right = new Vector(point.X + 1, point.Y);
    if (grid.ContainsKey(right)) pointsToTry.Add(right);

    foreach (var nextPoint in pointsToTry)
    {
        if (lowestScores.ContainsKey(nextPoint))
        {
            var newScore = score + lowestScores[nextPoint];
            if (newScore >= lowestScore) return;
            lowestScore = newScore;
            continue;
        }
        DepthScore(nextPoint, score + grid[nextPoint]);
    }
}

for (var y = finish.Y; y >= 0; y--)
{
    for (var x = finish.X; x >= 0; x--)
    {
        lowestScore = Int32.MaxValue;
        var point = new Vector(x, y);
        var riskToStart = point == new Vector(0, 0) ? 0 : grid[point];
        DepthScore(point, riskToStart);
        lowestScores.Add(point, lowestScore);
    }
}


Console.WriteLine(lowestScore);