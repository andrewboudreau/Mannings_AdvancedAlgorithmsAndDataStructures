namespace Chapter00;

public class Node(string key, double priority = 0d)
{
    public string Key { get; } = key;
    public double Priority { get; private set; } = priority;
    public Node? Parent { get; private set; }
    public Node? Left { get; private set; }
    public Node? Right { get; private set; }

    public void SetPriority(double priority)
    {
        Priority = priority;
    }

    public void SetParent(Node? node)
    {
        Parent = node;
    }

    public void SetLeft(Node? node)
    {
        Left = node;
        if (node is not null)
        {
            node.Parent = this;
        }
    }

    public void SetRight(Node? node)
    {
        Right = node;
        if (node is not null)
        {
            node.Parent = this;
        }
    }
}
