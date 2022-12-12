using Shared;

namespace Day12;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var lines = data.TrimEnd().Split("\n").ToList();
        
        var part1 = Part1(lines);
        var part2 = Part2(lines);

        return (part1, part2);
    }

    private static int Part1(List<string> lines)
    {
        var grid = new Dictionary<Vector, char>();

        var start = new Vector();
        var finish = new Vector();
        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var height = lines[y][x];
                var vector = new Vector(x, y);
                if (height == 'S')
                {
                    height = 'a';
                    start = vector;
                }

                if (height == 'E')
                {
                    height = 'z';
                    finish = vector;
                }

                grid.Add(vector, height);
            }
        }

        return ShortestPath(grid, start, finish);
    }
    
    private static int Part2(List<string> lines)
    {
        var grid = new Dictionary<Vector, char>();

        var finish = new Vector();
        for (var y = 0; y < lines.Count; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var height = lines[y][x];
                var vector = new Vector(x, y);
                if (height == 'S') height = 'a';
                if (height == 'E')
                {
                    height = 'z';
                    finish = vector;
                }

                grid.Add(vector, height);
            }
        }

        var shortest = int.MaxValue;
        foreach (var possibleStart in grid.Where(x => x.Value == 'a'))
        {
            var path = ShortestPath(grid, possibleStart.Key, finish, shortest);
            if (path < shortest) shortest = path;
        }

        return shortest;
    }

    private static int ShortestPath(Dictionary<Vector, char> grid, Vector start, Vector finish, int shortCircuit = Int32.MaxValue)
    {
        var distToVector = new Dictionary<Vector, int>();
        var queue = new PriorityQueue<Vector, int>();
        foreach (var item in grid)
        {
            if (item.Key == start) distToVector[item.Key] = 0;
            else distToVector[item.Key] = int.MaxValue;
        }

        queue.Enqueue(start, 0);
        while (queue.TryDequeue(out var item, out _))
        {
            var itemHeight = (int)grid[item];
            foreach (var adj in item.GetAdjacent(false))
            {
                // We've moved outside the bounds of the grid
                if (!grid.ContainsKey(adj)) continue;

                // if we can't travel to that point because it's too high, skip
                var adjHeight = (int)grid[adj];
                if (itemHeight + 1 < adjHeight) continue;

                var distToAdj = distToVector[item] + 1;

                // if the distance in this path is more than the short circuit, skip
                if (distToAdj >= shortCircuit) continue;
                
                // if the distance in this path is more than another path, skip
                if (distToAdj >= distToVector[adj]) continue;

                distToVector[adj] = distToAdj;
                queue.Enqueue(adj, distToAdj);
            }
        }

        return distToVector[finish];
    }
}