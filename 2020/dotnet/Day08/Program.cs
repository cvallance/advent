using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = CreateInstructions();
            var computer = new Computer(instructions);
            computer.Run();

            var second = 0;
            for (var i = 0; i < instructions.Count; i++)
            {
                var instruction = instructions[i];
                if (instruction.Operation == Operation.ACC)
                {
                    continue;
                }

                // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
                var newOperation = instruction.Operation switch
                {
                    Operation.JMP => Operation.NOP,
                    Operation.NOP => Operation.JMP,
                    _ => throw  new InvalidOperationException("Invalid operation")
                };

                var testInstructions = new List<Instruction>(instructions)
                {
                    [i] = new Instruction(newOperation, instruction.Argument)
                };
                var testComputer = new Computer(testInstructions);
                testComputer.Run();
                
                // ReSharper disable once InvertIf
                if (testComputer.ExitCode == 0)
                {
                    second = testComputer.Accumulator;
                    break;
                }
            }
            
            Console.WriteLine($"Day 8 - Part 1: {computer.Accumulator}");
            Console.WriteLine($"Day 8 - Part 2: {second}");
        }
        
        private static IList<Instruction> CreateInstructions()
        {
            var bootData = File.ReadAllLines("../../inputs/day8.txt");
            return ParseInstructions(bootData);
        }

        private static IList<Instruction> ParseInstructions(string[] bootData)
        {
            return bootData.Select(Instruction.Parse).ToList();
        }
    }

    public class Computer
    {
        
        private readonly IList<Instruction> _instructions;
        private readonly HashSet<int> _hasRun = new HashSet<int>();
        private int _instructionIndex = 0;
        
        public int ExitCode { get; private set; }
        public int Accumulator { get; private set; }
            
        public Computer(IList<Instruction> instructions)
        {
            _instructions = instructions;
        }

        public void Run()
        {
            while (true)
            {
                if (_instructionIndex >= _instructions.Count)
                {
                    break;
                }
                
                if (_hasRun.Contains(_instructionIndex))
                {
                    ExitCode = 1;
                    break;
                }
                
                _hasRun.Add(_instructionIndex);
                var currentInstruction = _instructions[_instructionIndex];
                switch (currentInstruction.Operation)
                {
                    case Operation.NOP:
                        _instructionIndex += 1;
                        break;
                    case Operation.ACC:
                        Accumulator += currentInstruction.Argument;
                        _instructionIndex += 1;
                        break;
                    case Operation.JMP:
                        _instructionIndex += currentInstruction.Argument;
                        break;
                    default:
                        throw new InvalidOperationException($"The current instruction operation is invalid - {currentInstruction.Operation}");
                }
            }
        }
    }

    public class Instruction
    {
        public Operation Operation { get; set; }
        public int Argument { get; set; }

        public Instruction(Operation operation, int argument)
        {
            Operation = operation;
            Argument = argument;
        }

        public static Instruction Parse(string instructionData)
        {
            var pieces = instructionData.Split(" ");
            return new Instruction(
                Enum.Parse<Operation>(pieces[0], true),
                int.Parse(pieces[1])
            );
        }
    }

    public enum Operation
    {
        NOP,
        ACC,
        JMP
    }
}
