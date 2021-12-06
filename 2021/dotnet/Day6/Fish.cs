namespace Day6;

public class Fish
{
    public int State { get; set; }
    private int LoopAge { get; set; } = 6;
    private int OffspringAge { get; set; } = 8;

    public Fish(int initialState)
    {
        State = initialState;
    }

    public Fish? Loop()
    {
        if (State == 0)
        {
            State = LoopAge;
            return new Fish(OffspringAge);
        }
        
        State -= 1;
        return null;
    }
}