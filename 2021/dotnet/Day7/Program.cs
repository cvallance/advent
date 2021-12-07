// var lines = File.ReadLines("../../inputs/day7-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day7.txt").ToList();
var items = lines[0].Split(",").Select(int.Parse).ToList();

var getLeastFuel = () =>
{
    var min = items.Min();
    var max = items.Max();
    // Fill lookup dictionary
    var moveCostLookup = new Dictionary<int, int>();
    var nextCost = 0;
    for (var i = 0; i <= max - min; i++)
    {
        moveCostLookup[i] = nextCost;
        nextCost += i + 1;
    }

    var getFuel = (int pos) =>
    {
        // return items.Select(x => Math.Abs(pos - x)).Sum();
        return items.Select(x => moveCostLookup[Math.Abs(pos - x)]).Sum();
    };
    
    var smallest = int.MaxValue;
    while (true)
    {
        var step = (max - min) / 3;
        var firstMid = min + step;
        var secondMid = min + step * 2;
        var minFuel = getFuel(min);
        var maxFuel = getFuel(max);
        var firstFuel = getFuel(firstMid);
        var secondFuel = getFuel(secondMid);
        var setMin = new List<int> { minFuel, maxFuel, firstFuel, secondFuel }.Min();
        if (setMin < smallest) smallest = setMin;
        
        if (firstFuel < secondFuel)
        {
            if (max == secondMid) return smallest;
            max = secondMid;
        }
        else
        {
            if (min == firstMid) return smallest;
            min = firstMid;
        }
    }
};

Console.WriteLine(getLeastFuel());