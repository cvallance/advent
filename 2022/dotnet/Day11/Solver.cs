namespace Day11;

public static class Solver
{
    public static (long, long) Solve(string data)
    {
        var lines = data.TrimEnd().Split("\n").ToList();

        var part1 = Process(lines, 20, true);
        var part2 = Process(lines, 10_000, false);
        return (part1, part2);
    }

    private static long Process(List<string> lines, int loops, bool isPartOne)
    {
        var monkeys = new List<Monkey>();
        var numMonkeys = lines.Count / 7;
        for (var i = 0; i <= numMonkeys; i++)
        {
            monkeys.Add(Monkey.CreateFromParsingShit(lines.Skip(i * 7 + 1).Take(5).ToList(), monkeys));
        }

        var lcm = FindLcm(monkeys.Select(x => x.DivTest).ToList());
        monkeys.ForEach(x => x.SetLCM(lcm));

        for (var i = 0; i < loops; i++)
        {
            foreach (var monkey in monkeys) monkey.TakeTurn(isPartOne);
        }

        var highest = monkeys.OrderByDescending(x => x.Inspections).First().Inspections;
        var second = monkeys.OrderByDescending(x => x.Inspections).Skip(1).First().Inspections;

        return highest * second;
    }

    private static long FindLcm(IList<int> divTests)
    {
        var max = divTests.Max();
        var counter = 1;
        while (true)
        {
            var test = max * counter;
            if (divTests.All(x => test % x == 0)) return test;

            counter++;
        }
    }

    public enum Operation
    {
        Addition,
        Multiplication,
        Square
    }
    
    public class Monkey
    {
        public readonly Queue<long> Items;
        public readonly Operation Operation;
        public readonly int DivTest;
        private readonly int _opValue;
        private readonly int _trueMonkey;
        private readonly int _falseMonkey;
        private readonly List<Monkey> _monkeys;
        private long _lcm;

        public long Inspections { get; private set; }

        private Monkey(Queue<long> items, Operation operation, int opValue, int divTest, int trueMonkey, int falseMonkey, List<Monkey> monkeys)
        {
            Items = items;
            Operation = operation;
            _opValue = opValue;
            DivTest = divTest;
            _trueMonkey = trueMonkey;
            _falseMonkey = falseMonkey;
            _monkeys = monkeys;
        }

        public void TakeTurn(bool isPartOne)
        {
            while (Items.Count > 0)
            {
                Inspections += 1;
                var item = Items.Dequeue();

                switch (Operation)
                {
                    case Operation.Addition:
                        item += _opValue;
                        break;
                    case Operation.Multiplication:
                        item *= _opValue;
                        break;
                    case Operation.Square:
                        item *= item;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (isPartOne) item /= 3;

                var nextMonkey = item % DivTest == 0
                    ? _monkeys[_trueMonkey]
                    : _monkeys[_falseMonkey];

                nextMonkey.Items.Enqueue(item % _lcm);
            }
        }

        public static Monkey CreateFromParsingShit(IList<string> shit, List<Monkey> monkeys)
        {
            //Shit on line 1
            var items = shit[0][17..].Split(", ").Select(long.Parse);
            
            //Shit on line 2
            var operation = shit[1].Contains('+')
                ? Operation.Addition
                : shit[1].Contains("old * old")
                    ? Operation.Square
                    : Operation.Multiplication;

            var opValue = operation is Operation.Addition or Operation.Multiplication
                ? int.Parse(shit[1][24..])
                : 0;

            //Shit on line 3
            var divTest = int.Parse(shit[2][20..]);
            
            //Shit on lines 4 and 5
            var trueMonkey = int.Parse(shit[3][28..]);
            var falseMonkey = int.Parse(shit[4][29..]);
            
            return new Monkey(new Queue<long>(items), operation, opValue, divTest, trueMonkey, falseMonkey, monkeys);
        }

        public void SetLCM(long lcm)
        {
            _lcm = lcm;
        }
    }        
}