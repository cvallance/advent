using Day14;

// var lines = File.ReadLines("../../inputs/day14-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day14.txt").ToList();

var values = lines[0].ToCharArray();
var insertLookup = new Dictionary<string, char>();
foreach (var line in lines.Skip(2))
{
    Console.WriteLine($"{line}");
    var parts = line.Split(" -> ");
    Console.WriteLine($"{parts[0]}-{parts[1]}");
    insertLookup[parts[0]] = char.Parse(parts[1]);
}

var loops = 40;
IDictionary<char, long> dictCount = values
    .GroupBy(x => x)
    .Select(x => new {x.Key, Count = x.Count()})
    .ToDictionary(x => x.Key, x => (long)x.Count);

// for (var i = 0; i < loops; i++)
// {
//     Console.WriteLine($"Loop {i + 1}");
//     
//     var newValues = new char[values.Length * 2 - 1];
//     for (var j = 0; j < values.Length - 1; j++)
//     {
//         var first = values[j];
//         var second = values[j+1];
//         newValues[j * 2] = first;
//         newValues[j * 2 + 2] = second;
//         var toInsert = insertLookup[$"{first}{second}"];
//         if (!dictCount.ContainsKey(toInsert)) dictCount[toInsert] = 0;
//         dictCount[toInsert] += 1;
//         newValues[j * 2 + 1] = toInsert;
//     }
//
//     values = newValues;
// }

var resultCache = new Dictionary<string, IDictionary<char, long>>();

Func<char,char,int,IDictionary<char, long>> depthScore = null!;
depthScore = (left, right, depth) =>
{
    if (depth > loops) return new Dictionary<char, long>();

    var key = $"{left}{right}{depth}";
    if (resultCache.TryGetValue(key, out var count)) return count;
    
    
    Console.WriteLine($"Left {left} right {right} depth {depth}");
    
    var toInsert = insertLookup[$"{left}{right}"];
    count = new Dictionary<char, long>
    {
        {toInsert, 1}
    };
    
    var leftDict = depthScore(left, toInsert, depth + 1);
    var rightDict = depthScore(toInsert, right, depth  + 1);

    var combinedDict = Helpers.CombineDicts(count, leftDict, rightDict);
    resultCache[key] = combinedDict;
    return combinedDict;
};

for (var i = 0; i < values.Length - 1; i++)
{
    Console.WriteLine($"Ok, here {i}");
    var result = depthScore(values[i], values[i + 1], 1);
    dictCount = Helpers.CombineDicts(dictCount, result);
}

Console.WriteLine(dictCount.Values.Max() - dictCount.Values.Min());
