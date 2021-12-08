namespace Day8;

[Flags]
public enum Tube
{
    None = 0,
    A = 1 << 0,
    B = 1 << 1,
    C = 1 << 2,
    D = 1 << 3,
    E = 1 << 4,
    F = 1 << 5,
    G = 1 << 6
}

public class DisplayNumber
{
    public int Number { get; init; }
    public Tube Tubes { get; init; }
}

// Thank you stack overflow ... https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
public class Helpers
{
    private static void Swap(ref char a, ref char b)
    {
        if (a == b) return;

        (a, b) = (b, a);
    }

    public static IEnumerable<char[]> GetPer(char[] list)
    {
        var x = list.Length - 1;
        return GetPer(list, 0, x);
    }

    private static IEnumerable<char[]> GetPer(char[] list, int k, int m)
    {
        if (k == m)
        {
            yield return list;
        }
        else
            for (var i = k; i <= m; i++)
            {
                Swap(ref list[k], ref list[i]);
                foreach (var loopList in GetPer(list, k + 1, m))
                {
                    yield return loopList;
                }
                Swap(ref list[k], ref list[i]);
            }
    }
}