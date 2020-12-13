using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shared;

namespace Day11
{
    class Program
    {
        private const char Empty = 'L';
        private const char Occupied = '#';
        private const char Floor = '.';

        private const string N = "N";
        private const string NE = "NE";
        private const string E = "E";
        private const string SE = "SE";
        private const string S = "S";
        private const string SW = "SW";
        private const string W = "W";
        private const string NW = "NW";
        private static readonly IList<string> Directions = new List<string> {N, NE, E, SE, S, SW, W, NW};
        
        static void Main(string[] args)
        {
            var layout = GetLayout();
            var stableLayout = LoopUntilStable(layout, 4);
            var first = stableLayout.SelectMany(x => x).Count(x => x == Occupied);
            Console.WriteLine($"Day 11 - Part 1: {first}");
            stableLayout = LoopUntilStable(layout, 5, true);
            var second = stableLayout.SelectMany(x => x).Count(x => x == Occupied);
            Console.WriteLine($"Day 11 - Part 2: {second}");
        }

        private static char[][] LoopUntilStable(char[][] layout, int occupiedTolerance, bool keepLooking = false)
        {
            var itemsChanged = false;
            var currentLayout = layout;
            do
            {
                var newLayout = new char[currentLayout.Length][];
                itemsChanged = false;

                var maxRow = currentLayout.Length - 1;
                foreach (var (currentRow, rowIndex) in currentLayout.WithIndex())
                {
                    var maxCol = currentRow.Length - 1;
                    newLayout[rowIndex] = new char[currentRow.Length];
                    foreach (var (col, colIndex) in currentRow.WithIndex())
                    {
                        if (col == Floor)
                        {
                            newLayout[rowIndex][colIndex] = col;
                            continue;
                        }

                        var occupiedCount = 0;
                        foreach (var direction in Directions)
                        {
                            var rowChk = rowIndex;
                            var colChk = colIndex;
                            while (true)
                            {
                                rowChk = direction switch
                                {
                                    N => rowChk - 1,
                                    NE => rowChk - 1,
                                    NW => rowChk - 1,
                                    SW => rowChk + 1,
                                    SE => rowChk + 1,
                                    S => rowChk + 1,
                                    _ => rowChk
                                };
                                if (rowChk == -1 || rowChk > maxRow) break;

                                colChk = direction switch
                                {
                                    E => colChk + 1,
                                    NE => colChk + 1,
                                    SE => colChk + 1,
                                    SW => colChk - 1,
                                    NW => colChk - 1,
                                    W => colChk - 1,
                                    _ => colChk
                                };
                                if (colChk == -1 || colChk > maxCol) break;

                                if (currentLayout[rowChk][colChk] == Floor && keepLooking) continue;
                                
                                if (currentLayout[rowChk][colChk] == Occupied) occupiedCount += 1;
                                
                                break;
                            }
                        }

                        var newState = col switch
                        {
                            Empty when occupiedCount == 0 => Occupied,
                            Occupied when occupiedCount >= occupiedTolerance => Empty,
                            _ => col
                        };

                        if (newState != col)
                        {
                            itemsChanged = true;
                        }
                        
                        newLayout[rowIndex][colIndex] = newState;
                    }
                }

                currentLayout = newLayout;
            } while (itemsChanged);

            return currentLayout;
        }

        private static char[][] GetLayout()
        {
            var rows = File.ReadAllLines("../../inputs/day11.txt");
            var layout = new char[rows.Length][];
            foreach (var (row, rowIndex) in rows.WithIndex())
            {
                layout[rowIndex] = row.ToCharArray();
            }

            return layout;
        }
    }
}
