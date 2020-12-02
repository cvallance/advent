using System;
using System.Collections.Generic;

namespace Day3
{
    public class Wire
    {
        public IList<Instruction> Instructions { get; set; } = new List<Instruction>();
        public IDictionary<Point, int> PathWithSteps { get; set; } = new Dictionary<Point, int>();

        public void ComputePath()
        {
            var last = new Point {x = 0, y = 0};
            var steps = 0;
            foreach (var instruction in Instructions)
            {
                Func<Point, Point> action;
                switch (instruction.Direction)
                {
                    case "U":
                        action = point => new Point {x = point.x, y = point.y + 1};
                        break;
                    case "D":
                        action = point => new Point {x = point.x, y = point.y - 1};
                        break;
                    case "L":
                        action = point => new Point {x = point.x - 1, y = point.y};
                        break;
                    case "R":
                        action = point => new Point {x = point.x + 1, y = point.y};
                        break;
                    default:
                        throw new Exception($"Invalid direction {instruction.Direction}");
                }

                var loops = instruction.Distance;
                while (loops > 0)
                {
                    last = action(last);
                    steps += 1;
                    if (!PathWithSteps.ContainsKey(last))
                    {
                        PathWithSteps[last] = steps;
                    }
                    loops -= 1;
                }
            }
        }
    }
}