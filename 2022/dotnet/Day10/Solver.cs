namespace Day10;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var part2 = 0;

        var signals = new List<int> {1};
        
        var cycle = 1;
        foreach (var line in lines)
        {
            var instruction = line[..4];
            if (instruction == "noop")
            {
                cycle += 1;
                signals.Add(signals[cycle - 2]);
            }
            else
            {
                var value = int.Parse(line[5..]);
                cycle++;
                signals.Add(signals[cycle - 2]);
                cycle++;
                signals.Add(signals[cycle - 2] + value);
            }
        }

        var part1 = 0;
        for (var i = 0; i < signals.Count; i++)
        {
            if (i < 40)
            {
                if (i + 1 == 20)
                {
                    part1 += signals[i] * (i + 1);
                }
            }
            else if ((i - 19) % 40 == 0)
            {
                part1 += signals[i] * (i + 1);
            }
        }
        
        for (var i = 0; i < signals.Count; i++)
        {
            if (i % 40 == 0)
            {
                Console.WriteLine("");
            }
            Console.Write("#");
        }
        
        return (part1, part2);
    }
}