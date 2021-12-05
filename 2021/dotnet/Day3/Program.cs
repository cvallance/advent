var lines = File.ReadLines("../../inputs/day3.txt").ToList();

// First
var lineLength = lines[0].Length;
var getOnCounts = (List<string> x) =>
{
    var y = new int[lineLength];
    foreach (var line in lines)
    {
        for (var i =0; i < lineLength; i++)
        {
            var value = line[i];
            if (value == '1') y[i] += 1;
        }
    }

    return y;
};

var midPoint = (double)lines.Count/2;
var gammaRateStr = "";
var epsilonRateStr = "";

var onCounts = getOnCounts(lines);
foreach (var onCount in onCounts)
{
    if (onCount > midPoint)
    {
        gammaRateStr += "1";
        epsilonRateStr += "0";
    }
    else
    {
        gammaRateStr += "0";
        epsilonRateStr += "1";
    }
}

var gammaRate = Convert.ToInt32(gammaRateStr, 2);
var epsilonRate = Convert.ToInt32(epsilonRateStr, 2);

Console.WriteLine(gammaRate * epsilonRate); // 2648450

// Second

var getOnCountsAtPos = (List<string> x, int pos) =>
{
    return x.Select(line => line[pos]).Count(value => value == '1');
};

var mutateList = (List<string> x, int pos, char toKeep) =>
{
    for (var y = x.Count - 1; y >= 0; y--)
    {
        var item = x[y];
        if (item[pos] != toKeep)
        {
            x.RemoveAt(y);
        }
    }
};

var oxyValues = lines.ToList();
var co2Values = lines.ToList();
for (var i = 0; i < lineLength; i++)
{
    if (oxyValues.Count > 1)
    {
        var oxyCounts = getOnCountsAtPos(oxyValues, i);
        var oxyMidPoint = (double)oxyValues.Count / 2;
        var mostPopular = oxyCounts >= oxyMidPoint ? '1' : '0';
        mutateList(oxyValues, i, mostPopular);
    }

    if (co2Values.Count > 1)
    {
        var co2Counts = getOnCountsAtPos(co2Values, i);
        var co2MidPoint = (double)co2Values.Count / 2;
        var leastPopular = co2Counts >= co2MidPoint ? '0' : '1';
        mutateList(co2Values, i, leastPopular);
    }
}

var oxyValue = Convert.ToInt32(oxyValues[0], 2);
var co2Value = Convert.ToInt32(co2Values[0], 2);

Console.WriteLine(oxyValue*co2Value); // 2845944
