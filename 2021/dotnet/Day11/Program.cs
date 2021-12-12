using Day11;

var octos = new Dictionary<Vector, int>();
// var lines = File.ReadLines("../../inputs/day11-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day11.txt").ToList();
for (var y = 0; y < lines.Count; y++)
{
    var line = lines[y];
    for (var x = 0; x < lines.Count; x++)
    {
        octos.Add(new Vector(x, y), int.Parse(lines[y][x].ToString()));
    }
}

var flashes = 0;
for (var stepCount = 0; stepCount < 1000; stepCount++)
{
    // Up them all first
    foreach (var octo in octos.Keys)
    {
        octos[octo] += 1;
    }
    
    bool flash;
    do
    {
        flash = false;
        foreach (var octo in octos.Keys)
        {
            if (octos[octo] >= 10)
            {
                flash = true;
                flashes += 1;
                octos[octo] = 0;
                foreach (var adOcto in octo.GetAdjacent(true))
                {
                    if (octos.ContainsKey(adOcto) && octos[adOcto] != 0) 
                    {
                        octos[adOcto] += 1;
                    }
                }
            }
        }
    } while (flash);

    if (octos.Values.Distinct().Count() == 1)
    {
        Console.WriteLine(stepCount + 1);
        break;
    }
}

Console.WriteLine(flashes);