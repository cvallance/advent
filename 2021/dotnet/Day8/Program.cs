using Day8;
// var lines = File.ReadLines("../../inputs/day8-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day8.txt").ToList();

var zeroCount = 6; // abcefg - no d
var oneCount = 2; // cf  - UNIQUE
var twoCount = 5; // acdeg - no bf
var threeCount = 5; // acdfg - no be
var fourCount = 4; // bcdf - no aeg - UNIQUE
var fiveCount = 4; // abdfg - no ce
var sixCount = 4; // abdefg - no c
var sevenCount = 3; // acf - UNIQUE
var eightCount = 7; // abcedfg - all - UNIQUE
var nineCount = 6; // abcdfg - no e


var firstLengths = new[] { oneCount, fourCount, sevenCount, eightCount };
var firstCount = 0;
foreach (var line in lines)
{
    var parts = line.Split(" | ");
    firstCount += parts[1].Split(" ").Count(x => firstLengths.Contains(x.Length));
}

Console.WriteLine(firstCount);

// given cf and acf we can figure out a == ?
// given sets of 6, we can figure out d and e
// Ah fuck it... let's just use bits

var zero = new DisplayNumber
{
    Number = 0,
    Tubes = Tube.A | Tube.B | Tube.C | Tube.E | Tube.F | Tube.G
};
var one = new DisplayNumber
{
    Number = 1,
    Tubes = Tube.C | Tube.F
};
var two = new DisplayNumber
{
    Number = 2,
    Tubes = Tube.A | Tube.C | Tube.D | Tube.E | Tube.G,
};
var three = new DisplayNumber
{
    Number = 3,
    Tubes = Tube.A | Tube.C | Tube.D | Tube.F | Tube.G,
};
var four = new DisplayNumber
{
    Number = 4,
    Tubes =  Tube.B | Tube.C | Tube.D | Tube.F,
};
var five = new DisplayNumber
{
    Number = 5,
    Tubes =  Tube.A | Tube.B | Tube.D | Tube.F | Tube.G,
};
var six = new DisplayNumber
{
    Number = 6,
    Tubes =  Tube.A | Tube.B | Tube.D | Tube.E | Tube.F | Tube.G,
};
var seven = new DisplayNumber
{
    Number = 7,
    Tubes =  Tube.A | Tube.C | Tube.F,
};
var eight = new DisplayNumber
{
    Number = 8,
    Tubes =  Tube.A | Tube.B | Tube.C | Tube.D | Tube.E | Tube.F | Tube.G,
};
var nine = new DisplayNumber
{
    Number = 9,
    Tubes =  Tube.A | Tube.B | Tube.C | Tube.D | Tube.F | Tube.G,
};
var allNumbers = new[] { zero, one, two, three, four, five, six, seven, eight, nine };

var permutations = Helpers.GetPer(new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' });
var total = 0;
foreach (var line in lines)
{
    foreach (var permutation in permutations)
    {
        var mappings = new Dictionary<char, Tube>()
        {
            { permutation[0], Tube.A },
            { permutation[1], Tube.B },
            { permutation[2], Tube.C },
            { permutation[3], Tube.D },
            { permutation[4], Tube.E },
            { permutation[5], Tube.F },
            { permutation[6], Tube.G }
        };
        var parts = line.Split(" | ");
        
        var invalidNumberFound = false;
        foreach (var firstPart in parts[0].Split(" "))
        {
            Tube number = 0;
            foreach (var firstPartTube in firstPart)
            {
                number |= mappings[firstPartTube];
            }

            if (allNumbers.Any(x => (x.Tubes ^ number) == Tube.None)) continue;

            invalidNumberFound = true;
            break;
        }

        if (invalidNumberFound)
        {
            continue;
        }

        var endNumber = new string[4];
        var secondParts = parts[1].Split(" ");
        for (var i = 0; i < secondParts.Length; i++)
        {
            Tube number = 0;
            foreach (var firstPartTube in secondParts[i])
            {
                number |= mappings[firstPartTube];
            }

            var realNumber = allNumbers.Single(x => (x.Tubes ^ number) == Tube.None);
            endNumber[i] = realNumber.Number.ToString();
        }


        total += int.Parse($"{endNumber[0]}{endNumber[1]}{endNumber[2]}{endNumber[3]}");
    }

    if (total > 0)
    {
        Console.WriteLine(total);
    }
}
