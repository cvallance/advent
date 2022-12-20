using System.Diagnostics;

namespace Day16;

public static class Solver
{
    private static readonly Dictionary<string, Valve> Valves = new();
    private const string StartValve = "AA";
    
    public static (int, int) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var sw = Stopwatch.StartNew();
        var rootValve = ParseStuff(lines);
        Console.WriteLine($"Parsing {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        var part1 = Part1(rootValve);
        Console.WriteLine($"Part1 {sw.ElapsedMilliseconds}ms");
        
        sw.Restart();
        var part2 = Part2(rootValve);
        Console.WriteLine($"Part2 {sw.ElapsedMilliseconds}ms");
        
        return (part1, part2);
    }

    private static Valve ParseStuff(IList<string> lines)
    {
        foreach (var line in lines)
        {
            var firstStart = 6;
            var firstEnd = line.IndexOf(' ', firstStart);
            var name = line.Substring(firstStart, firstEnd - firstStart);
            var secondStart = line.IndexOf('=');
            var secondEnd = line.IndexOf(';', secondStart);
            var flow = int.Parse(line.Substring(secondStart +1, secondEnd - secondStart - 1));
            var leadsTo = line.Substring(secondEnd + 24).Trim().Split(", ").ToHashSet();

            Valves[name] = new Valve(name, flow, leadsTo);
        }

        foreach (var (startName, startValve) in Valves.Where(x => x.Key == StartValve|| x.Value.FlowRate > 0))
        {
            var distToValve = new Dictionary<string, int>();
            foreach (var valve in Valves.Values)
            {
                distToValve[valve.Name] = valve.Name == startName ? 0 : int.MaxValue;
            }
            
            // Do a breadth first search to figure out shortest distance from each valve to each other valve
            var queue = new PriorityQueue<Valve, int>();
            queue.Enqueue(startValve, 0);
            while (queue.TryDequeue(out var item, out _))
            {
                foreach (var adj in item.LeadsTo)
                {
                    var existingDistToItem = distToValve[adj]; 
                    var distToItem = distToValve[item.Name]; 
                    var newDistToAdj = distToItem + 1;
                    if (newDistToAdj >= existingDistToItem) continue;

                    distToValve[adj] = newDistToAdj;
                    queue.Enqueue(Valves[adj], newDistToAdj);
                }
            }
            
            // Populate the valves shortest distance lookup
            var otherValves = Valves.Where(x => x.Key != startName && x.Value.FlowRate > 0);
            foreach (var (otherName, _) in otherValves)
            {
                var shortestDist = distToValve[otherName]; 
                startValve.ShortestDistances[otherName] = shortestDist;
            }
        }

        return Valves[StartValve];
    }

    private static int Part1(Valve rootValve)
    {
        var paths = new List<Path>();
        
        void FindPath(Path path, Valve currentValve, int time)
        {
            // Loop over every potential valve to open
            foreach (var (nextValveName, nextValveDistance) in currentValve.ShortestDistances)
            {
                // If the valve is already open, move on
                if (path.ValvesOpen.ContainsKey(nextValveName)) continue;

                var newTime = time + nextValveDistance;
                // If the new time is going to be over 30, move on
                if (newTime >= 30) continue;
                
                var nextValve = Valves[nextValveName];
                var newPath = path.Clone();
                paths.Add(newPath);
                newPath.ValvesOpen[nextValveName] = newTime;
                FindPath(newPath, nextValve, newTime + 1);
            }
        }

        var firstPath = new Path();
        paths.Add(firstPath);
        FindPath(firstPath, rootValve, 1);

        return paths.Max(x => x.CalculatePressure());
    }

    private static int Part2(Valve rootValve)
    {
        
        var paths = new List<Path>();
        
        void FindPath(Path path, Valve currentValve, int time)
        {
            // Loop over every potential valve to open
            foreach (var (nextValveName, nextValveDistance) in currentValve.ShortestDistances)
            {
                // If the valve is already open, move on
                if (path.ValvesOpen.ContainsKey(nextValveName)) continue;

                var newTime = time + nextValveDistance;
                // If the new time is going to be over 30, move on
                if (newTime >= 26) continue;

                // if (time < 10) paths.Remove(path);

                var nextValve = Valves[nextValveName];
                var newPath = path.Clone();
                paths.Add(newPath);
                newPath.ValvesOpen[nextValveName] = newTime;
                FindPath(newPath, nextValve, newTime + 1);
            }
        }

        var firstPath = new Path();
        paths.Add(firstPath);
        FindPath(firstPath, rootValve, 1);
        
        // Just put it all back through... lets pair it down first
        var pathsToCheck = paths.OrderByDescending(x => x.CalculatePressureAtTime(26)).Take(1000).ToList();
        foreach (var path in pathsToCheck)
        {
            FindPath(path, rootValve, 1);
        }

        return paths.Max(x => x.CalculatePressureAtTime(26));
    }

    public record Valve(
        string Name,
        int FlowRate,
        HashSet<string> LeadsTo)
    {
        public readonly Dictionary<string, int> ShortestDistances = new();

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    public class Path
    {
        public readonly Dictionary<string, int> ValvesOpen;

        public Path() : this(new Dictionary<string, int>()) { }

        private Path(Dictionary<string, int> valvesOpen)
        {
            ValvesOpen = valvesOpen;
        }

        public Path Clone()
        {
            return new Path(new Dictionary<string, int>(ValvesOpen));
        }

        public int CalculatePressure()
        {
            return CalculatePressureAtTime(30);
        }

        public int CalculatePressureAtTime(int timeLimit)
        {
            var pressure = 0;
            foreach (var valveOpen in ValvesOpen)
            {
                var valveName = valveOpen.Key;
                var time = valveOpen.Value;
                if (time > timeLimit) continue;
                pressure += Valves[valveName].FlowRate * (timeLimit - time);
            }

            return pressure;
        }
    }
}