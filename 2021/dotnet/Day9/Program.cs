var lines = File
    .ReadLines("../../inputs/day9.txt")
    // .ReadLines("../../inputs/day9-test.txt")
    .Select(
        x => x
            .ToCharArray()
            .Select(y => int.Parse(y.ToString())).ToList())
    .ToList();

var rowCount = lines.Count;
var colCount = lines[0].Count;

Func<int, int, int> findBasinSize = null!;
findBasinSize = (int row, int col) =>
{
    var current = lines[row][col];
    if (current == 9) return 0;
    
    var count = 1;
    lines[row][col] = -1;
    var up = row - 1 >= 0 ? lines[row - 1][col] : -1;
    if (up > current) count += findBasinSize!(row - 1, col);
    
    var down = row + 1 < rowCount ? lines[row + 1][col] : -1;
    if (down > current) count += findBasinSize!(row + 1, col);
    
    var left = col - 1 >= 0 ? lines[row][col - 1] : -1;
    if (left > current) count += findBasinSize!(row, col - 1);
    
    var right = col + 1 < colCount ? lines[row][col + 1] : -1;
    if (right > current) count += findBasinSize!(row, col + 1);
    
    return count;
};

var riskTotal = 0;
var basinSizes = new List<int>();
for (var row = 0; row < rowCount; row++)
{
    for (var col = 0; col < colCount; col++)
    {
        var current = lines[row][col];
        if (current == -1) continue;
        
        // Up
        var up = row - 1 >= 0 ? lines[row - 1][col] : int.MaxValue;
        var down = row + 1 < rowCount ? lines[row + 1][col] : int.MaxValue;
        var left = col - 1 >= 0 ? lines[row][col - 1] : int.MaxValue;
        var right = col + 1 < colCount ? lines[row][col + 1] : int.MaxValue;
        var all = new[] {up, down, left, right};
        if (all.All(x => x > current))
        {
            riskTotal += current + 1;
            basinSizes.Add(findBasinSize(row, col));
        }
    }
}

Console.WriteLine(riskTotal);
var top3 = basinSizes.OrderByDescending(x => x).Take(3).ToList();
Console.WriteLine(top3[0] * top3[1] * top3[2]);
