namespace Day6;

public class FishTwo
{
    private readonly long[] _counts = new long[9];
    
    public FishTwo(int initialState)
    {
        _counts[initialState] = 1;
    }

    public FishTwo(IEnumerable<int> initialStates)
    {
        foreach (var initialState in initialStates)
        {
            _counts[initialState] += 1;
        }
    }

    public long Total(int days)
    {
        for (var i = 0; i < days; i++)
        {
            for (var y = 0; y < 8; y++)
            {
                _counts[y] ^= _counts[y+1];
                _counts[y+1] = _counts[y] ^ _counts[y+1];
                _counts[y] ^= _counts[y+1];
                if (y == 7)
                {
                    _counts[6] += _counts[8];
                }
            }
        }

        return _counts.Sum();
    }
}