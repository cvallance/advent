namespace Shared;

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

    private static readonly List<Vector> Adjacent = new()
    {
        new Vector(0, 1),
        new Vector(1, 0),
        new Vector(0, -1),
        new Vector(-1, 0)
    };
    
    private static readonly List<Vector> AdjacentDiagonal = new()
    {
        new Vector(0, 1), new Vector(1, 1),
        new Vector(1, 0), new Vector(1, -1),
        new Vector(0, -1), new Vector(-1, -1),
        new Vector(-1, 0), new Vector(-1, 1)
    };
    
    public IEnumerable<Vector> GetAdjacent(bool includeDiagonal)
    {
        var adjacentItems = includeDiagonal ? AdjacentDiagonal : Adjacent;
        foreach (var adItem in adjacentItems)
        {
            yield return this + adItem;
        }
    }

    public override string ToString()
    {
        return $"x={X} y={Y}";
    }
}