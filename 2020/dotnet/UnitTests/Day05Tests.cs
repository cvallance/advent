using Day05;
using Xunit;

namespace UnitTests
{
    public class Day05Tests
    {
        [Theory]
        [InlineData("BFFFBBFRRR", 70, 7, 567)]
        [InlineData("FFFBBBFRRR", 14, 7, 119)]
        [InlineData("BBFFBBFRLL", 102, 4, 820)]
        public void Test1(string seatStr, int row, int column, int seatId)
        {
            var seat = Seat.Parse(seatStr);
            Assert.Equal(row, seat.Row);
            Assert.Equal(column, seat.Column);
            Assert.Equal(seatId, seat.SeatId);
        }
    }
}
