using Day12;

// var lines = File.ReadLines("../../inputs/day12-test.txt").ToList();
var lines = File.ReadLines("../../inputs/day12.txt").ToList();

var nodes = new Dictionary<string, Node>();
foreach (var line in lines)
{
    var parts = line.Split("-");
    var parent = new Node {Name = parts[0]};
    if (nodes.ContainsKey(parent.Name))
    {
        parent = nodes[parent.Name];
    }
    else
    {
        nodes.Add(parent.Name, parent);
    }
    
    var child = new Node {Name = parts[1]};
    if (nodes.ContainsKey(child.Name))
    {
        child = nodes[child.Name];
    }
    else
    {
        nodes.Add(child.Name, child);
    }
    
    parent.Children.Add(child);
    child.Children.Add(parent);
}

var completePaths = new List<Day12.Path>();
Action<Day12.Path> findPaths = null!;
findPaths = currentPath =>
{
    var tailNode = currentPath.TailNode;
    foreach (var childNode in tailNode.Children)
    {
        var newPath = currentPath.Clone();
        
        if (!newPath.AddNode(childNode)) continue;
        
        if (newPath.IsComplete())
        {
            completePaths.Add(newPath);
            continue;
        }

        findPaths(newPath);
    }
};

if (!nodes.ContainsKey("start")) throw new Exception("????");

var startingPath = new Day12.Path(nodes["start"]);
findPaths(startingPath);

Console.WriteLine(completePaths.Count);