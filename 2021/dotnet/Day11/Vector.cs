namespace Day11;

public struct Vector
{
    public int X { get; init; }
    public int Y { get; init; }

    public Vector(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(X.GetHashCode(), Y.GetHashCode());
    }

    public static Vector operator +(Vector a, Vector b)
    {
        return new Vector {X = a.X + b.X, Y = a.Y + b.Y};
    }

    public static bool operator ==(Vector a, Vector b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    public static bool operator !=(Vector a, Vector b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Vector vectorObj) return this == vectorObj;
        
        return base.Equals(obj);
    }

    private static List<Vector> _adjacent = new List<Vector>
    {
        new Vector(0, 1),
        new Vector(1, 0),
        new Vector(0, -1),
        new Vector(-1, 0)
    };
    
    private static List<Vector> _adjacentDiagonal = new List<Vector>
    {
        new Vector(0, 1), new Vector(1, 1), // 0,1 1,1
        new Vector(1, 0), new Vector(1, -1), // 1,0 1,-1
        new Vector(0, -1), new Vector(-1, -1), // 0,-1, -1, -1
        new Vector(-1, 0), new Vector(-1, 1) // -1,0 -1.1
    };
    
    public IEnumerable<Vector> GetAdjacent(bool includeDiagonal)
    {
        var adjacentItems = includeDiagonal ? _adjacentDiagonal : _adjacent;
        foreach (var adItem in adjacentItems)
        {
            yield return this + adItem;
        }
    }
}