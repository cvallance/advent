// var lines = File.ReadLines("../../inputs/day10-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day10.txt").ToList();

var openToClose = new Dictionary<char, char> {{'(', ')'}, {'[', ']'}, {'{', '}'}, {'<', '>'}};
var firstPrice = new Dictionary<char, int> {{')', 3}, {']', 57}, {'}', 1197}, {'>', 25137}};
var secondPrice = new Dictionary<char, int> {{')', 1}, {']', 2}, {'}', 3}, {'>', 4}};
var firstCount = 0;
var secondValues = new List<long>();
foreach (var line in lines)
{
    var charStack = new Stack<char>();
    var isCurrupt = false;
    foreach (var lineChar in line.Trim())
    {
        if (openToClose.ContainsKey(lineChar))
        {
            charStack.Push(lineChar);
            continue;
        }

        if (charStack.Count == 0)
        {
            continue;
        }
        
        var lastOpen = charStack.Pop();
        if (openToClose[lastOpen] == lineChar)
        {
            // It's legit
            continue;
        }

        isCurrupt = true;
        firstCount += firstPrice[lineChar];
        break;
    }

    if (!isCurrupt)
    {
        var lineScore = 0L;
        while (charStack.TryPop(out var lastOpen))
        {
            lineScore *= 5;
            lineScore += secondPrice[openToClose[lastOpen]];
        }
        
        secondValues.Add(lineScore);
    }
}

Console.WriteLine(firstCount);
var sorted = secondValues.OrderBy(x => x).ToList();
Console.WriteLine(sorted[sorted.Count / 2]);
