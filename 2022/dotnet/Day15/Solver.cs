using System.Numerics;
using Vector = Shared.Vector;

namespace Day15;

public static class Solver
{
    public static (int, int) Solve(string data, int rowToTest)
    {
        var lines = data.Split("\n").ToList();

        var part1 = 0;
        var part2 = 0;

        var sensors = ParseSensors(lines).ToList();
        var beacons = new HashSet<Vector>();
        foreach (var sensorAndBacon in sensors)
        {
            beacons.Add(sensorAndBacon.Item2);
        }
        
        foreach (var sensorAndBacon in sensors)
        {
            var sensor = sensorAndBacon.Item1;
            var beacon = sensorAndBacon.Item2;
            var beaconYAway = sensor.Y < beacon.Y
                ? beacon.Y - sensor.Y
                : sensor.Y - beacon.Y;
            var beaconXAway = sensor.X < beacon.X
                ? beacon.X - sensor.X
                : sensor.X - beacon.X;
            var top = new Vector(sensor.X, sensor.Y - beaconYAway - beaconXAway);
            var move = 0;
            while (move >= 0)
            {
                var point = new Vector(top.X, y)
                move++;
            }
            if (sensor.X == 8 && sensor.Y == 7)
            {
                Console.WriteLine($"Sensor {sensor}");
                Console.WriteLine($"Beacon {beacon}");
                Console.WriteLine($"top {top}");
            }
        }
        
        return (part1, part2);
    }

    private static IEnumerable<(Vector, Vector)> ParseSensors(IEnumerable<string> lines)
    {
        var result = new List<(Vector, Vector)>();
        foreach (var line in lines)
        {
            var firstStart = line.IndexOf('=');
            var firstEnd = line.IndexOf(',', firstStart);
            var first = int.Parse(line.Substring(firstStart +1, firstEnd - firstStart - 1));
            var secondStart = line.IndexOf('=', firstEnd);
            var secondEnd = line.IndexOf(':', secondStart);
            var second = int.Parse(line.Substring(secondStart +1, secondEnd - secondStart - 1));
            var thirdStart = line.IndexOf('=', secondEnd);
            var thirdEnd = line.IndexOf(',', thirdStart);
            var third = int.Parse(line.Substring(thirdStart +1, thirdEnd - thirdStart - 1));
            var fourthStart = line.IndexOf('=', thirdEnd);
            var fourth = int.Parse(line.Substring(fourthStart +1));
            
            result.Add((new Vector(first, second), new Vector(third, fourth)));
        }
        return result;
    }
}