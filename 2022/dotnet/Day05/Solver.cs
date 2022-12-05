namespace Day05;

public static class Solver
{
    public static (string, string) Solve(string data)
    {
        var lines = data.Split("\n").ToList();
        var part1Stacks = GetStacks(lines);
        foreach (var line in lines.Where(x => x.StartsWith("move")))
        {
            var parts = line.Split(" ");
            var numItems = int.Parse(parts[1]);
            var from = int.Parse(parts[3]);
            var to = int.Parse(parts[5]);
            for (var i = numItems; i > 0; i--) part1Stacks[to-1].Push(part1Stacks[from-1].Pop());
        }

        var part1 = "";
        foreach (var stack in part1Stacks)
        {
            if (stack.TryPeek(out var top)) part1 += top;
                
        }
        
        var part2Stacks = GetStacks(lines);
        foreach (var line in lines.Where(x => x.StartsWith("move")))
        {
            var parts = line.Split(" ");
            var numItems = int.Parse(parts[1]);
            var from = int.Parse(parts[3]);
            var to = int.Parse(parts[5]);
            var interStack = new Stack<string>();
            for (var i = numItems; i > 0; i--) interStack.Push(part2Stacks[from - 1].Pop());
            foreach (var inter in interStack) part2Stacks[to-1].Push(inter);
        }

        var part2 = "";
        foreach (var stack in part2Stacks)
        {
            if (stack.TryPeek(out var top)) part2 += top;
        }
        
        return (part1, part2);
    }

    private static List<Stack<string>> GetStacks(IEnumerable<string> lines)
    {
        var stacks = new List<Stack<string>>();
        lines = lines.Where(x => !x.StartsWith("move")).ToList();
        foreach (var line in lines.Reverse())
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            for (var i = 1; i < line.Length; i += 4)
            {
                var value = line[i].ToString();
                if (string.IsNullOrWhiteSpace(value)) continue;
                
                if (int.TryParse(value, out _)) stacks.Add(new Stack<string>());
                else stacks[(i - 1) / 4].Push(value);
            }
        }

        return stacks;
    }
}