namespace Day06;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var part1 = 0;
        var part2 = 0;
        for (var i = 0; i < data.Length - 4; i++)
        {
            var set = new HashSet<char>(data.Skip(i).Take(4));
            var setTwo = new HashSet<char>(data.Skip(i).Take(14));
            if (part1 == 0 && set.Count == 4)
            {
                part1 = i + 4;
            }
            if (setTwo.Count == 14)
            {
                part2 = i + 14;
            }
        }
        
        //TODO
        
        return (part1, part2);
    }
}