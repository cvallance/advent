// var limits = new[] {20,30,-10,-5};
var limits = new[] {79,137,-176,-117};

// First

var currentX = 0;
var xVelocity = 0;
while (currentX < limits[1])
{
    if (currentX > limits[0]) break;
    xVelocity += 1;
    currentX += xVelocity;
}

var maxYVelocity = Math.Abs(limits[2]) - 1;
var minYVelocity = limits[2];

var lastY = 0;
var highestY = 0;
var yVelocity = maxYVelocity;
while (lastY >= highestY)
{
    lastY += yVelocity;
    yVelocity -= 1;
    if (lastY > highestY) highestY = lastY;
}

Console.WriteLine($"{highestY}");

// Second
var workingYs = new Dictionary<int, List<int>>();
for (var y = minYVelocity; y <= maxYVelocity; y++)
{
    var steps = 0;
    yVelocity = y;
    var currentY = 0;
    while (currentY >= limits[2])
    {
        steps += 1;
        currentY += yVelocity;
        if (currentY <= limits[3] && currentY >= limits[2])
        {
            if (!workingYs.ContainsKey(y)) workingYs[y] = new List<int>();
            workingYs[y].Add(steps);
        }
        yVelocity -= 1;
    }
}

var maxSteps = workingYs.Max(x => x.Value.Max());
var workingXs = new Dictionary<int, List<int>>();
var maxXVelocity = limits[1];
for (var x = 1; x <= maxXVelocity; x++)
{
    var steps = 0;
    xVelocity = x;
    currentX = 0;
    while (currentX <= limits[1] && steps <= maxSteps)
    {
        steps += 1;
        currentX += xVelocity;
        if (currentX >= limits[0] && currentX <= limits[1])
        {
            if (!workingXs.ContainsKey(x)) workingXs[x] = new List<int>();
            workingXs[x].Add(steps);
        }

        if (xVelocity > 0) xVelocity -= 1;
    }
}

var combinationCount = 0;
foreach (var workingX in workingXs)
{
    foreach (var workingY in workingYs)
    {
        if (workingY.Value.Any(x => workingX.Value.Contains(x)))
        {
            combinationCount += 1;
        }
    }
}

Console.WriteLine(combinationCount);