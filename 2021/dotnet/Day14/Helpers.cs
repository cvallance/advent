namespace Day14;

public static class Helpers
{
    public static IDictionary<char, long> CombineDicts(params IDictionary<char, long>[] dicts)
    {
        var toReturn = new Dictionary<char, long>();
        foreach (var dict in dicts)
        {
            foreach (var kvp in dict)
            {
                if (!toReturn.ContainsKey(kvp.Key)) toReturn[kvp.Key] = 0;
                toReturn[kvp.Key] += kvp.Value;
            }
        }

        return toReturn;
    }
}