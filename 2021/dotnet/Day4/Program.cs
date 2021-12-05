using Day4;

// var lines = File.ReadLines("../../inputs/day4-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day4.txt").ToList();
var numbers = lines[0].Split(",").Select(int.Parse).ToList();
var boards = new List<Board>();
for (var i = 2; i < lines.Count; i += 6)
{
    boards.Add(new Board(lines.GetRange(i,5)));
}

var firstWinTotal = 0;
var lastWinTotal = 0;
foreach (var number in numbers)
{
    var winningBoards = boards.Where(board => board.MarkNumber(number) && board.HasWon()).ToList();
    foreach (var board in winningBoards)
    {
        if (firstWinTotal == 0)
        {
            Console.WriteLine("First winning board:");
            board.PrintBoard();
            firstWinTotal = board.UnmarkedSum() * number;
        }

        boards.Remove(board);

        if (boards.Count == 0)
        {
            Console.WriteLine("Last winning board:");
            board.PrintBoard();
            lastWinTotal = board.UnmarkedSum() * number;
        }
    }
}

Console.WriteLine(firstWinTotal); // 35670
Console.WriteLine(lastWinTotal); // 22704
