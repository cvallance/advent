using System.Diagnostics;
using System.Numerics;
using System.Text;
using Shared;
using Vector = Shared.Vector;

namespace Day15;

public static class Solver
{
    public static (int, BigInteger) Solve(string data, int rowToTest, int maxSize)
    {
        var lines = data.Split("\n").ToList();

        var sw = Stopwatch.StartNew();
        var sensors = ParseSensors(lines).ToList();
        Console.WriteLine($"Parsing {sw.ElapsedMilliseconds}ms");
            
        sw.Restart();
        var part1 = Part1(sensors, rowToTest);
        Console.WriteLine($"Part1 {sw.ElapsedMilliseconds}ms");
        
        sw.Restart();
        var part2 = Part2(sensors, maxSize);
        Console.WriteLine($"Part2 {sw.ElapsedMilliseconds}ms");
        
        return (part1, part2);
    }

    private static int Part1(List<(Vector, Vector)> sensors, int rowToTest)
    {
        var beacons = new HashSet<Vector>();
        foreach (var sensorAndBacon in sensors)
        {
            beacons.Add(sensorAndBacon.Item2);
        }
        
        var cantBe = new HashSet<Vector>();

        void AddToCantBe(Vector vec)
        {
            if (beacons!.Contains(vec)) return;

            cantBe!.Add(vec);
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
            
            var top = new Vector(sensor.X, sensor.Y + beaconYAway + beaconXAway);
            var bottom = new Vector(sensor.X, sensor.Y - beaconYAway - beaconXAway);

            if (rowToTest > top.Y || rowToTest < bottom.Y)
            {
                continue;
            }
            
            var width = rowToTest > sensor.Y ? top.Y - rowToTest : rowToTest - bottom.Y;
            var point = new Vector(sensor.X, rowToTest);
            AddToCantBe(point);
            for (var x = 1; x <= width; x++)
            {
                AddToCantBe(point + new Vector(x, 0));
                AddToCantBe(point + new Vector(-x, 0));
            }
        }

        var part1 = cantBe.Count(x => x.Y == rowToTest);
        return part1;
    }

    private static BigInteger Part2(List<(Vector, Vector)> sensors, int maxSize)
    {
        var lines = new List<List<HorizontalLine>>(maxSize);
        for (var x = 0; x <= maxSize; x++)
        {
            lines.Add(new List<HorizontalLine>());
        }

        foreach (var sensorAndBacon in sensors)
        {
            var sensor = sensorAndBacon.Item1;
            var beacon = sensorAndBacon.Item2;
            var beaconYAway = Math.Abs(beacon.Y - sensor.Y);
            var beaconXAway = Math.Abs(beacon.X - sensor.X);
            var beaconAway = beaconXAway + beaconYAway;
            var top = new Vector(sensor.X, sensor.Y + beaconAway);
            
            var width = 0;
            var point = top;
            while (width >= 0)
            {
                if (point.Y < 0 || point.Y > maxSize)
                {
                    width = point.Y > sensor.Y ? width + 1 : width - 1;
                    point += new Vector(0, -1);
                    continue;
                }
                
                var lineStart = point + new Vector(-width, 0);
                var lineEnd = point + new Vector(width, 0);
                var newLine = new HorizontalLine(lineStart, lineEnd);

                var existingLines = lines[point.Y];
                if (existingLines.Count == 0) existingLines.Add(newLine);
                
                var newLines = new List<HorizontalLine>();
                foreach (var existingLine in existingLines)
                {
                    if (newLine.CanJoin(existingLine)) newLine += existingLine;
                    else newLines.Add(existingLine);
                }
                newLines.Add(newLine);
                lines[point.Y] = newLines;

                width = point.Y > sensor.Y ? width + 1 : width - 1;
                point += new Vector(0, -1);
            }
        }

        foreach (var hLines in lines)
        {
            if (hLines.Count == 1) continue;

            var first = hLines[0];
            var second = hLines[1];

            var xCoOrd = first.Start.X < second.Start.X
                ? first.End.X + 1
                : second.End.X + 1;
            
            return new BigInteger(xCoOrd * 4_000_000L) + first.Start.Y;
        }

        return 1;
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
            var fourth = int.Parse(line[(fourthStart + 1)..]);
            
            result.Add((new Vector(first, second), new Vector(third, fourth)));
        }
        return result;
    }
}