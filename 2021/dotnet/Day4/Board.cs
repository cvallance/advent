namespace Day4;

public class Board
{
    private class BoardItem
    {
        public int Value { get; init; }
        public bool Found { get; set; }

        private char FoundChar => Found ? 'T' : 'F';
        public override string ToString()
        {
            return $"{Value}{FoundChar}";
        }
    }
    
    private readonly BoardItem[,] _items = new BoardItem[5,5];
    private readonly HashSet<int> _itemHashSet = new();
    
    public Board(IList<string> boardLines)
    {
        if (boardLines.Count != 5) throw new ArgumentOutOfRangeException(nameof(boardLines));

        for (var x = 0; x < 5; x++)
        {
            var lineValues = boardLines[x].Split(" ").Where(y => y != string.Empty).Select(int.Parse).ToList();
            for (var y = 0; y < 5; y++)
            {
                _items[x, y] = new BoardItem { Value = lineValues[y] };
                _itemHashSet.Add(lineValues[y]);
            }
        }
    }

    private int _lastX;
    private int _lastY;
    public bool MarkNumber(int num)
    {
        if (!_itemHashSet.Contains(num)) return false;

        for (var x = 0; x < 5; x++)
        {
            for (var y = 0; y < 5; y++)
            {
                var item = _items[x, y];
                if (item.Value != num) continue;
                
                item.Found = true;
                _lastX = x;
                _lastY = y;
                return true;
            }
        }

        throw new Exception("We should have found it by now");
    }
    
    public bool HasWon()
    {
        var foundXCount = 0;
        var foundYCount = 0;
        for (var i = 0; i < 5; i++)
        {
            if (_items[_lastX, i].Found) foundXCount++;
            if (_items[i, _lastY].Found) foundYCount++;
        }

        return foundXCount == 5 || foundYCount == 5;
    }

    public int UnmarkedSum()
    {
        return (
            from BoardItem item in _items
            where !item.Found
            select item
        ).Sum(x => x.Value);
    }

    public void PrintBoard()
    {
        for (var x = 0; x < 5; x++)
        {
            Console.WriteLine($"{_items[x,0]}\t{_items[x,1]}\t{_items[x,2]}\t{_items[x,3]}\t{_items[x,4]}");
        }
    }
}