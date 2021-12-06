namespace Day5;

public readonly struct Point
{
    public int X { get; init; }
    public int Y { get; init; }

    public override string ToString()
    {
        return $"x{X},y{Y}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}