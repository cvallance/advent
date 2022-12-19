using System.Diagnostics;

namespace Day16;

public static class Solver
{
    private static Dictionary<string, Valve> s_valves = new();
    
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

            s_valves[name] = new Valve(name, flow, leadsTo);
        }
        return (s_valves, s_valves[rootValve]);
    }

    private static int Part1(IDictionary<string, Valve> valves, Valve rootValve)
    {
        var paths = new List<Path>();
        
        // Breadth first search maintaining a lookup of the best flows for each valve at a given minute
        void FindPath(Path path, Valve currentValve, int time)
        {
            // If we've gone over time, return
            if (time > 30) return;
            
            // Console.WriteLine($"{time} currentValve {currentValve}");
            
            // If there's another path that's already better, return
            var currentPathPressure = path.CalculatePressure();
            if (paths.Any(x => x.IsBetter(currentValve.Name, currentPathPressure, time)))
            {
                paths.Remove(path);
                return;
            }

            // Ok, remove the path a put it's permutations in
            paths.Remove(path);
            
            
            // Can we open the current valve?
            if (currentValve.FlowRate > 0 && !path.IsValveOpen(currentValve.Name))
            {
                // Console.WriteLine($" opening currentValve {currentValve}");
                var newPath = path.Clone();
                newPath.OpenValve(currentValve.Name, time);
                paths.Add(newPath);
                FindPath(newPath, currentValve, time + 1);
            }

            foreach (var nextValveName in currentValve.LeadsTo)
            {
                var nextValve = s_valves[nextValveName];
                // Console.WriteLine($" moving to {currentValve}");
                var newPath = path.Clone();
                paths.Add(newPath);
                FindPath(newPath, nextValve, time + 1);
            }
        }

        var firstPath = new Path();
        paths.Add(firstPath);
        FindPath(firstPath, rootValve, 1);
        
        Console.WriteLine($"Paths {paths.Count}");
        Console.WriteLine($"Max {paths.Max(x => x.CalculatePressure())}");
        return paths.Max(x => x.CalculatePressure());
    }

    private static int Part2(IDictionary<string, Valve> valves, Valve rootValve)
    {
        return 1;
    }

    public record Valve(string Name, int FlowRate, HashSet<string> LeadsTo);

    public class Path
    {
        public readonly Dictionary<int, string> ValvesOpen;

        public Path() : this(new Dictionary<int, string>()) { }

        private Path(Dictionary<int, string> valvesOpen)
        {
            ValvesOpen = valvesOpen;
        }

        public Path Clone()
        {
            return new Path(new Dictionary<int, string>(ValvesOpen));
        }
        
        public bool IsValveOpen(string valveName)
        {
            return ValvesOpen.Any(x => x.Value == valveName);
        }
        
        public void OpenValve(string valveName, int time)
        {
            ValvesOpen[time] = valveName;
        }
        
        public bool IsBetter(string otherPathValveName, int otherPathPressure, int time)
        {
            // // We only compare if they're on the same valve at the same time
            // if (!ValvesOpen.TryGetValue(time, out var valveName) || valveName != otherPathValveName) return false;
            // return CalculatePressureAtTime(time) > otherPathPressure;
            
            return ValvesOpen.Any(
                x => x.Key < time 
                     && x.Value == otherPathValveName 
                     && CalculatePressureAtTime(x.Key) > otherPathPressure);
        }
        
        public int CalculatePressureAtTime(int atTime)
        {
            var pressure = 0;
            foreach (var (time, valveName) in ValvesOpen)
            {
                if (time > atTime) continue;
                pressure += (30 - time) * s_valves[valveName].FlowRate;
            }

            return pressure;
        }

        public int CalculatePressure()
        {
            var pressure = 0;
            foreach (var valveOpen in ValvesOpen)
            {
                var time = valveOpen.Key;
                var valveName = valveOpen.Value;
                pressure += (30 - time) * s_valves[valveName].FlowRate;
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
                
                if (ValvesOpen.TryGetValue(i, out var valveName))
                {
                    valvesOpen.Add(valveName);
                    pressure += s_valves[valveName].FlowRate;
                    Console.WriteLine($"You open valve {valveName}");
                }
                
                Console.WriteLine();
            }
        }
    }
}