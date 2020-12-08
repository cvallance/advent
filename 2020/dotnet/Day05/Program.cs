using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        static void Main(string[] args)
        {
            var seats = ParseSeats();
            var first = seats.Max(x => x.SeatId);
            var allSeatsHashSet = seats.Select(x => x.SeatId).ToHashSet();
            var second = 0;
            for (var row = 0; row <= 127; row ++)
            {
                if (second != 0) break;
                for (var column = 0; column <= 7; column++)
                {
                    var seat = new Seat(row, column);
                    var seatId = seat.SeatId;
                    if (!allSeatsHashSet.Contains(seatId) && allSeatsHashSet.Contains(seatId+1) && allSeatsHashSet.Contains(seatId -1))
                    {
                        second = seatId;
                    }
                }
            }
            
            Console.WriteLine($"Day 5 - Part 1: {first}");
            Console.WriteLine($"Day 5 - Part 2: {second}");
        }
        
        private static List<Seat> ParseSeats()
        {
            var seats = new List<Seat>();
            var lines = File.ReadLines("../../inputs/day5.txt");
            foreach (var line in lines)
            {
                seats.Add(Seat.Parse(line));
            }

            return seats;
        }
    }

    public class Seat
    {
        public int Row { get; }
        public int Column { get; }
        public int SeatId => Row * 8 + Column;

        public Seat(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static Seat Parse(string seatStr)
        {
            var rowStr = seatStr.Substring(0, 7);
            var row = ParseLocation(rowStr.ToCharArray(), 'F', 'B', 0, 127);
            var columnStr = seatStr.Substring(7);
            var column = ParseLocation(columnStr.ToCharArray(), 'L', 'R', 0, 7);
            
            return new Seat(row, column);
        }

        private static int ParseLocation(char[] chars, char lowerChar, char upperChar, int lowerBounds, int upperBounds)
        {
            foreach (var xchar in chars)
            {
                var toMove = (int) Math.Floor((upperBounds - lowerBounds) / 2m);
                if (xchar == lowerChar)
                {
                    upperBounds = lowerBounds + toMove;
                }
                else if (xchar == upperChar)
                {
                    lowerBounds = upperBounds - toMove;
                }
            }

            return lowerBounds;
        }
    }
}
