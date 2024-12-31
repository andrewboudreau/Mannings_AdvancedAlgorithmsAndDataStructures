namespace Chapter00;

public class TreapEntry<TValue, TPriority>(TValue value, TPriority priority)
{
    public TValue Value { get; } = value;
    public TPriority Priority { get; private set; } = priority;
    public TreapEntry<TValue, TPriority>? Parent { get; private set; }
    public TreapEntry<TValue, TPriority>? Left { get; private set; }
    public TreapEntry<TValue, TPriority>? Right { get; private set; }

    public void SetPriority(TPriority priority)
    {
        Priority = priority;
    }

    public void SetParent(TreapEntry<TValue, TPriority>? node)
    {
        Parent = node;
    }

    public void SetLeft(TreapEntry<TValue, TPriority>? node)
    {
        Left = node;
        if (node is not null)
        {
            node.Parent = this;
        }
    }

    public void SetRight(TreapEntry<TValue, TPriority>? node)
    {
        Right = node;
        if (node is not null)
        {
            node.Parent = this;
        }
    }

    public override string ToString()
    {
        return $"{Value} P:{Priority} L:{(Left != null ? Left.Value : '_')} R:{(Right != null ? Right.Value : '_')} ";
    }
}
