using System;
using Shared;

namespace Day2
{
    class Program
    {
        private static readonly int[] MasterIntCodes = Intcode.ParseFile("../../day2_input.txt");
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        private static void PartOne()
        {
            var intCodes = new int[MasterIntCodes.Length];
            MasterIntCodes.CopyTo(intCodes, 0);
            intCodes[1] = 12;
            intCodes[2] = 2;
            ProcessIntCode(intCodes);
            Console.WriteLine($"Part 1: {intCodes[0]}");
        }
        
        private static void PartTwo()
        {
            for (var noun = 1; noun < 100; noun++)
            {
                for (var verb = 1; verb < 100; verb++)
                {
                    var intCodes = new int[MasterIntCodes.Length];
                    MasterIntCodes.CopyTo(intCodes, 0);
                    intCodes[1] = noun;
                    intCodes[2] = verb;
                    ProcessIntCode(intCodes);
                    if (intCodes[0] != 19690720) continue;
                    
                    Console.WriteLine($"Part 2: {100 * noun + verb}");
                    return;
                }
            }
        }

        private static void ProcessIntCode(int[] intCodes)
        {
            var instructionPointer = 0;
            while (true)
            {
                var opCode = intCodes[instructionPointer];
                // Console.WriteLine($"Opcode {opCode} at address {instructionPointer} with parameters {intCodes[instructionPointer + 1]}, {intCodes[instructionPointer + 2]}, {intCodes[instructionPointer + 3]}");
                switch (opCode)
                {
                    case 1:
                        Case1(intCodes, instructionPointer);
                        instructionPointer += 4;
                        break;
                    case 2:
                        Case2(intCodes, instructionPointer);
                        instructionPointer += 4;
                        break;
                    case 99:
                        return;
                    default:
                        throw new Exception($"Invalid value at position {instructionPointer}");
                }
            }
        }

        private static void Case1(int[] intCodes, int position)
        {
            intCodes[intCodes[position + 3]] = intCodes[intCodes[position + 1]] + intCodes[intCodes[position + 2]];
        }
        
        private static void Case2(int[] intCodes, int position)
        {
            intCodes[intCodes[position + 3]] = intCodes[intCodes[position + 1]] * intCodes[intCodes[position + 2]];
        }
    }
}