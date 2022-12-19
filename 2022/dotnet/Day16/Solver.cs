using System.Diagnostics;

namespace Day16;

public static class Solver
{
    private static readonly Dictionary<string, Valve> Valves = new();
    
    public static (int, int) Solve(string data)
    {
        var lines = data.Split("\n").ToList();

        var sw = Stopwatch.StartNew();
        var (valves, rootValve) = ParseStuff(lines);
        Console.WriteLine($"Parsing {sw.ElapsedMilliseconds}ms");

        sw.Restart();
        var part1 = Part1(valves, rootValve);
        Console.WriteLine($"Part1 {sw.ElapsedMilliseconds}ms");
        
        sw.Restart();
        var part2 = Part2(valves, rootValve);
        Console.WriteLine($"Part2 {sw.ElapsedMilliseconds}ms");
        
        return (part1, part2);
    }

    private static (IDictionary<string, Valve> allValves, Valve) ParseStuff(IList<string> lines)
    {
        var rootValve = string.Empty;
        foreach (var line in lines)
        {
            var firstStart = 6;
            var firstEnd = line.IndexOf(' ', firstStart);
            var name = line.Substring(firstStart, firstEnd - firstStart);
            var secondStart = line.IndexOf('=');
            var secondEnd = line.IndexOf(';', secondStart);
            var flow = int.Parse(line.Substring(secondStart +1, secondEnd - secondStart - 1));
            var leadsTo = line.Substring(secondEnd + 24).Trim().Split(", ").ToHashSet();
            if (string.IsNullOrEmpty(rootValve)) rootValve = name;

            Valves[name] = new Valve(name, flow, leadsTo);
        }

        foreach (var (startName, startValve) in Valves.Where(x => x.Key == rootValve || x.Value.FlowRate > 0))
        {
            var distToValve = new Dictionary<string, int>();
            var queue = new PriorityQueue<Valve, int>();
            foreach (var valve in Valves.Values)
            {
                distToValve[valve.Name] = valve.Name == startName ? 0 : int.MaxValue;
            }
            
            queue.Enqueue(startValve, 0);
            while (queue.TryDequeue(out var item, out _))
            {
                foreach (var adj in item.LeadsTo)
                {
                    var distToAdj = distToValve[item.Name] + 1;
                    if (distToAdj >= distToValve[adj]) continue;

                    distToValve[adj] = distToAdj;
                    queue.Enqueue(Valves[adj], distToAdj);
                }
            }
            
            var otherValves = Valves.Where(x => x.Key != startName && x.Value.FlowRate > 0);
            foreach (var (otherName, _) in otherValves)
            {
                startValve.ShortestDistances[otherName] = distToValve[otherName];
            }
        }
        return (Valves, Valves[rootValve]);
    }

    private static int Part1(IDictionary<string, Valve> valves, Valve rootValve)
    {
        var paths = new List<Path>();
        
        void FindPath(Path path, Valve currentValve, int time)
        {
            // If we've gone over time, return
            if (time > 30) return;
            
            foreach (var (nextValveName, nextValveDistance) in currentValve.ShortestDistances)
            {
                if (path.ValvesOpen.ContainsKey(nextValveName)) continue;
                
                var nextValve = Valves[nextValveName];
                var newPath = path.Clone();
                paths.Add(newPath);
                newPath.ValvesOpen[nextValveName] = time + nextValveDistance;
                FindPath(newPath, nextValve, time + nextValveDistance + 1);
            }
        }

        var firstPath = new Path();
        paths.Add(firstPath);
        FindPath(firstPath, rootValve, 1);

        var bestPath = paths.OrderByDescending(x => x.CalculatePressure()).First();
        bestPath.Print();
        return paths.Max(x => x.CalculatePressure());
    }

    private static int Part2(IDictionary<string, Valve> valves, Valve rootValve)
    {
        return 1;
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
        
        public bool IsValveOpen(string valveName)
        {
            return ValvesOpen.Any(x => x.Key == valveName);
        }
        
        public void OpenValve(string valveName, int time)
        {
            ValvesOpen[valveName] = time;
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
                pressure += Valves[valveName].FlowRate * (30 - time);
            }

            return pressure;
        }

        public void Print()
        {
            var valvesOpen = new List<string>();
            var pressure = 0;
            for (var i = 1; i <= 30; i++)
            {
                Console.WriteLine($"== Minute {i} ==");
                Console.WriteLine(valvesOpen.Any() 
                    ? $"Valve(s) {string.Join(", ", valvesOpen)} are open, releasing {pressure} pressure." 
                    : "No valves are open.");
                Console.WriteLine($"Running pressure {CalculatePressureAtTime(i)}");

                if (ValvesOpen.Any(x => x.Value == i))
                {
                    var openingValve = ValvesOpen.SingleOrDefault(x => x.Value == i);
                    var valveName = openingValve.Key;
                    valvesOpen.Add(valveName);
                    pressure += Valves[valveName].FlowRate;
                    Console.WriteLine($"You open valve {valveName}");
                }
                
                Console.WriteLine();
            }
        }
    }
}