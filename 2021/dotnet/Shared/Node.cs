namespace Shared;

public struct Node
{
    public string Name { get; init; }
    public bool IsLarge => Name.ToUpper() == Name;

    public HashSet<Node> Children = new();

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

public struct Path
{
    public bool DoubleUsed = false;
    public List<Node> Nodes { get; init; } = new();
    public HashSet<Node> SmallNodes { get; init; } = new();

    public Node TailNode => Nodes[^1];

    public Path() { }
    public Path(Node startingNode)
    {
        Nodes.Add(startingNode);
    }
    
    public bool IsComplete()
    {
        if (Nodes.Count == 0) return false;

        return TailNode.Name == "end";
    }

    public bool AddNode(Node node)
    {
        if (!node.IsLarge)
        {
            if (node.Name == "start") return false;
            
            if (SmallNodes.Contains(node))
            {
                if (DoubleUsed) return false;

                DoubleUsed = true;
            }
            SmallNodes.Add(node);
        }

        Nodes.Add(node);
        return true;
    }
    
    public Path Clone()
    {
        return new Path {Nodes = Nodes.ToList(), SmallNodes = new HashSet<Node>(SmallNodes), DoubleUsed = DoubleUsed};
    }

    public override int GetHashCode()
    {
        var hashCode = 0;
        foreach (var node in Nodes)
        {
            hashCode = HashCode.Combine(hashCode, node.GetHashCode());
        }
        return hashCode;
    }

    public override string ToString()
    {
        return string.Join(" -> ", Nodes.Select(x => x.Name));
    }
}