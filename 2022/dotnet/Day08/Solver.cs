namespace Day08;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        data = data.Trim();
        var lines = data.Split("\n").ToList();

        var part1 = PartOne(lines);
        var part2 = PartTwo(lines);

        return (part1, part2);
    }

    public static int PartOne(List<string> lines)
    {
        var maxX = lines[0].Length;
        var maxY = lines.Count;

        var part1 = maxX * 2 + maxY * 2;
        part1 -= 4;

        bool Visible(int x, int y, Func<int, int> xMutator, Func<int, int> yMutator)
        {
            var treeHeight = int.Parse(lines![y][x].ToString());
            var searchX = x;
            var searchY = y;
            do
            {
                searchX = xMutator(searchX);
                searchY = yMutator(searchY);
                var searchTreeHeight = int.Parse(lines[searchY][searchX].ToString());
                if (searchTreeHeight >= treeHeight) return false;
            } while (searchX > 0 && searchX < maxX - 1 && searchY > 0 && searchY < maxY - 1);

            return true;
        }

        for (var y = 1; y < maxY - 1; y++)
        {
            for (var x = 1; x < maxX - 1; x++)
            {
                if (Visible(x, y, a => a + 1, b => b))
                {
                    part1 += 1;
                    continue;
                }

                if (Visible(x, y, a => a - 1, b => b))
                {
                    part1 += 1;
                    continue;
                }

                if (Visible(x, y, a => a, b => b + 1))
                {
                    part1 += 1;
                    continue;
                }

                if (Visible(x, y, a => a, b => b - 1))
                {
                    part1 += 1;
                }
            }
        }

        return part1;
    }

    public static int PartTwo(List<string> lines)
    {
        var maxX = lines[0].Length;
        var maxY = lines.Count;

        int TreesVisible(int x, int y, Func<int, int> xMutator, Func<int, int> yMutator)
        {
            var treeHeight = int.Parse(lines![y][x].ToString());
            var searchX = x;
            var searchY = y;
            var treesVisible = 0;
            do
            {
                treesVisible += 1;
                searchX = xMutator(searchX);
                searchY = yMutator(searchY);
                var searchTreeHeight = int.Parse(lines[searchY][searchX].ToString());
                if (searchTreeHeight >= treeHeight) return treesVisible;
            } while (searchX > 0 && searchX < maxX - 1 && searchY > 0 && searchY < maxY - 1);

            return treesVisible;
        }

        var highestScore = 0;
        for (var y = 1; y < maxY - 1; y++)
        {
            for (var x = 1; x < maxX - 1; x++)
            {
                var scenicScore =
                    TreesVisible(x, y, a => a + 1, b => b)
                    * TreesVisible(x, y, a => a - 1, b => b)
                    * TreesVisible(x, y, a => a, b => b + 1)
                    * TreesVisible(x, y, a => a, b => b - 1);

                if (scenicScore > highestScore) highestScore = scenicScore;
            }
        }

        return highestScore;
    }
}