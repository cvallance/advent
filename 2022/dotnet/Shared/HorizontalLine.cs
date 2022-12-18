namespace Shared;

public struct HorizontalLine
{
    public Vector Start { get; }
    public Vector End { get; }

    public HorizontalLine(Vector start, Vector end)
    {
        if (start.Y != end.Y) throw new Exception("Different Y values, line not horizontal");
        
        // Try and make them always left to right
        Start = start.X < end.X ? start : end;
        End = start.X < end.X ? end : start;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Start.GetHashCode(), End.GetHashCode());
    }

    public static HorizontalLine operator +(HorizontalLine a, HorizontalLine b)
    {
        if (!a.CanJoin(b)) throw new Exception("Cannot combine lines that do no overlap");

        var newStart = new Vector(Math.Min(a.Start.X, b.Start.X), a.Start.Y);
        var newEnd = new Vector(Math.Max(a.End.X, b.End.X), a.End.Y);
        return new HorizontalLine(newStart, newEnd);
    }
    
    public static bool operator ==(HorizontalLine a, HorizontalLine b)
    {
        return a.Start == b.Start && a.End == b.End;
    }

    public static bool operator !=(HorizontalLine a, HorizontalLine b)
    {
        return !(a == b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is HorizontalLine lineObj) return this == lineObj;
        
        return base.Equals(obj);
    }

    public bool CanJoin(HorizontalLine line)
    {
        if (Start.Y != line.Start.Y) return false; // throw?

        if (line.Start.X >= Start.X && line.Start.X <= End.X + 1) return true;
        if (line.End.X >= Start.X - 1 && line.End.X <= End.X) return true;
        if (Start.X >= line.Start.X && Start.X <= line.End.X + 1) return true;
        if (End.X >= line.Start.X - 1 && End.X <= line.End.X) return true;

        return false;
    }

    public override string ToString()
    {
        return $"start=[{Start}] end=[{End}]";
    }
}