// var lines = File.ReadLines("../../inputs/day5-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day5.txt").ToList();

var points = new Dictionary<Point, int>();
foreach (var line in lines)
{
    var items = line.Split(" -> ");
    var first = items[0].Split(",").Select(int.Parse).ToList();
    var second = items[1].Split(",").Select(int.Parse).ToList();

    var x1 = first[0];
    var y1 = first[1];
    var x2 = second[0];
    var y2 = second[1];
    if (x1 == x2)
    {
        var start = y1;
        var end = y2;
        if (y1 > y2)
        {
            start = y2;
            end = y1;
        }

        for (var i = start; i <= end; i++)
        {
            var point = new Point { X = x1, Y = i };
            if (!points.ContainsKey(point))
            {
                points[point] = 0;
            }

            points[point] += 1;
        }
    }
    
    if (y1 == y2)
    {
        var start = x1;
        var end = x2;
        if (x1 > x2)
        {
            start = x2;
            end = x1;
        }

        for (var i = start; i <= end; i++)
        {
            var point = new Point { X = i, Y = y1 };
            if (!points.ContainsKey(point))
            {
                points[point] = 0;
            }

            points[point] += 1;
        }
    }

    var distance = Math.Abs(x1 - x2);
    if (Math.Abs(x1 - x2) == Math.Abs(y1 - y2))
    {
        if (x1 - x2 == y1 - y2)
        {
            // plus y
            var startX = x1;
            var startY = y1;
            if (x2 < x1)
            {
                startX = x2;
                startY = y2;
            }

            var j = startY;
            for (var i = startX; i <= startX + distance; i++, j++)
            {
                var point = new Point { X = i, Y = j };
                if (!points.ContainsKey(point))
                {
                    points[point] = 0;
                }

                points[point] += 1;
            }
        }
        else
        {
            // minus y
            var startX = x1;
            var startY = y1;
            if (x2 < x1)
            {
                startX = x2;
                startY = y2;
            }

            var j = startY;
            for (var i = startX; i <= startX + distance; i++, j--)
            {
                var point = new Point { X = i, Y = j };
                if (!points.ContainsKey(point))
                {
                    points[point] = 0;
                }

                points[point] += 1;
            }
        }
    }
}

var doubleCount = points.Values.Count(x => x > 1);

Console.WriteLine(doubleCount);
