public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public override string ToString()
    {
        return $"x{X},y{Y}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}