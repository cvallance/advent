using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day12
{
    public class Program
    {
        private const string North = "N";
        private const string South = "S";
        private const string West = "W";
        private const string East = "E";
        private const string Forward = "F";
        private const string Left = "L";
        private const string Right = "R";

        private static readonly string[] Directions = {East, South, West, North};
        
        static void Main(string[] args)
        {
            var instructions = GetInstructions().ToList();
            var first = FirstSolution(instructions);
            Console.WriteLine($"Day 12 - Part 1: {first}");
            var second = SecondSolution(instructions);
            Console.WriteLine($"Day 12 - Part 2: {second}");
        }

        public static int FirstSolution(IEnumerable<(string, int)> instructions)
        {
            var location = new Point(0, 0);
            var facing = 0;
            foreach (var (instruction, param) in instructions)
            {
                var loopInstruction = instruction == Forward
                    ? Directions[facing]
                    : instruction;

                switch (loopInstruction)
                {
                    case North:
                        location.Y += param;
                        break;
                    case South:
                        location.Y -= param;
                        break;
                    case West:
                        location.X -= param;
                        break;
                    case East:
                        location.X += param;
                        break;
                    case Left:
                    case Right:
                        var movements = param / 90;
                        facing = loopInstruction == Left
                            ? facing - movements
                            : facing + movements;
                        while (facing < 0) facing += 4;
                        facing %= 4;
                        break;
                }
            }

            return Math.Abs(location.X) + Math.Abs(location.Y);
        }

        public static int SecondSolution(IEnumerable<(string, int)> instructions)
        {
            var location = new Point(0, 0);
            var waypoint = new Point(10, 1);
            foreach (var (instruction, param) in instructions)
            {
                Console.WriteLine($"Instruction:{instruction} Param:{param}");
                switch (instruction)
                {
                    case Forward:
                        location.X += waypoint.X * param;
                        location.Y += waypoint.Y * param;
                        break;
                    case North:
                        waypoint.Y += param;
                        break;
                    case South:
                        waypoint.Y -= param;
                        break;
                    case West:
                        waypoint.X -= param;
                        break;
                    case East:
                        waypoint.X += param;
                        break;
                    case Left:
                    case Right:
                        var movements = param / 90;
                        var oldX = waypoint.X;
                        var oldY = waypoint.Y;
                        switch (movements) 
                        {
                            case 1 when instruction == Left:
                            case 3 when instruction == Right:
                                waypoint.X = oldY * -1;
                                waypoint.Y = oldX;
                                break;
                            case 1 when instruction == Right:
                            case 3 when instruction == Left:
                                waypoint.X = oldY;
                                waypoint.Y = oldX * -1;
                                break;
                            case 2:
                                waypoint.X *= -1;
                                waypoint.Y *= -1;
                                break;
                            default:
                                throw new Exception("WAT");
                        }
                        break;
                }

                Console.WriteLine($"WayPo X:{waypoint.X} Y:{waypoint.Y}");
                Console.WriteLine($"Local X:{location.X} Y:{location.Y}");
            }

            return Math.Abs(location.X) + Math.Abs(location.Y);
        }

        private static IEnumerable<(string, int)> GetInstructions()
        {
            return ParseInstructions(File.ReadAllText("../../inputs/day12.txt"));
        }
        
        public static IEnumerable<(string, int)> ParseInstructions(string instructions)
        {
            return instructions.Trim().Split("\n").Select(line => (line.Substring(0, 1), int.Parse(line.Substring(1))));
        }
    }
}
