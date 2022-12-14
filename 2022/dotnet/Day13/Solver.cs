namespace Day13;

public static class Solver
{
    public static (int, int) Solve(string data)
    {
        var pairs = data.Split("\n\n").ToList();

        var part1 = 0;
        var part2 = 0;

        var itemsIndex = 1;
        var rightOrder = new List<int>();
        var allRootItems = new List<Item>();
        foreach (var pair in pairs)
        {
            var eh = pair.Split("\n").ToList();
            var left = eh[0];
            var right = eh[1];
            var leftRootItem = parseItems(left);
            var rightRootItem = parseItems(right);
            if (leftRootItem.CompareTo(rightRootItem) != 1) rightOrder.Add(itemsIndex);
            itemsIndex++;
            
            // Part 2
            allRootItems.Add(leftRootItem);
            allRootItems.Add(rightRootItem);
        }

        var firstDivPacket = parseItems("[[2]]");
        var secondDivPacket = parseItems("[[6]]");
        allRootItems.Add(firstDivPacket);
        allRootItems.Add(secondDivPacket);
        allRootItems.Sort();
        var firstIndex = allRootItems.IndexOf(firstDivPacket) + 1;
        var secondIndex = allRootItems.IndexOf(secondDivPacket) + 1;
        return (rightOrder.Sum(), firstIndex * secondIndex);
    }

    private static Item parseItems(string input)
    {
        Item? rootItem = null;
        var itemStack = new Stack<Item>();
        var pos = 0;
        do
        {
            var charAtPos = input[pos];
            switch (charAtPos)
            {
                // If we're creating a new the current item, just move on
                case '[':
                {
                    var newList = new Item(ItemType.List);
                    if (itemStack.TryPeek(out var parent)) parent.Children.Add(newList);
                    rootItem ??= newList;
                    itemStack.Push(newList);
                    pos++;
                    continue;
                }
                // If we're moving to another item, just continue
                case ',':
                    pos++;
                    continue;
            }
            
            // If we're closing the current item, just move on
            if (charAtPos == ']')
            {
                pos++;
                _ = itemStack.Pop();
                continue;
            }
            
            var item = itemStack.Peek();
            // Get the number
            var searchTo = input.IndexOfAny(new[] { ',', ']' }, pos);
            var numberStr = input.Substring(pos, searchTo - pos);
            var number = int.Parse(numberStr);
            item.Children.Add(new Item(ItemType.Number) {Number = number});
            pos = searchTo;
        }
        while (itemStack.Count > 0);

        return rootItem!;
    }

    private enum ItemType
    {
        Number, List
    }
    
    private class Item : IComparable
    {
        public ItemType ItemType { get; }
        public int? Number { get; set; }
        public List<Item> Children { get; } = new();

        public Item(ItemType itemType)
        {
            ItemType = itemType;
        }

        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(this, obj)) return 0;
            if (obj is not Item item) return 1;

            return compareItems(this, item);
        }

        private int compareItems(Item leftItem, Item rightItem)
        {
            while (true)
            {
                if (leftItem.ItemType == ItemType.Number && rightItem.ItemType == ItemType.Number)
                {
                    if (leftItem.Number! < rightItem.Number!) return -1;
                    if (leftItem.Number! > rightItem.Number!) return 1;
                }
                else if (leftItem.ItemType == ItemType.List && rightItem.ItemType == ItemType.List)
                {
                    var leftEnumerator = leftItem.Children.GetEnumerator();
                    var rightEnumerator = rightItem.Children.GetEnumerator();
                    while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
                    {
                        var left = leftEnumerator.Current;
                        var right = rightEnumerator.Current;
                        var itemComparison = compareItems(left, right);
                        if (itemComparison != 0) return itemComparison;
                    }

                    if (leftItem.Children.Count < rightItem.Children.Count) return -1;
                    if (leftItem.Children.Count > rightItem.Children.Count) return 1;
                }
                else
                {
                    if (leftItem.ItemType == ItemType.Number)
                    {
                        var newItem = new Item(ItemType.List);
                        newItem.Children.Add(leftItem);
                        leftItem = newItem;
                    }
                    else
                    {
                        var newItem = new Item(ItemType.List);
                        newItem.Children.Add(rightItem);
                        rightItem = newItem;
                    }

                    continue;
                }

                return 0;
            }
        }
    }
}