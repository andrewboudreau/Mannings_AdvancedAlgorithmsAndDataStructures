
// Figure 2.6
var tasks = new Pair[8]
{
    new Pair(10, "Unencrypted password on DB"),
    new Pair(9, "UI breaks on browser X"),
    new Pair(8, "Optional form field blocked"),
    new Pair(8, "CSS style causes misalignment"),
    new Pair(7, "Page loads take 2+ seconds"),
    new Pair(5, "CSS style causes 1px offset"),
    new Pair(3, "Refactor CSS using SASS"),
    new Pair(9, "Memory leak")
};

var heap = new DHeap(tasks);


heap.Print();
heap.BubbleUp();

public record Pair(int Priority, string Element);

public class DHeap
{
    private const int BranchingFactor = 3;
    private readonly Pair[] pairs;
    private Pair swapSpace;

    public DHeap(params Pair[] pairs)
    {
        this.pairs = pairs;
    }

    public void BubbleUp(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
        if (index > pairs.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

        int parentIndex = index;
        int currentIndex;

        while (parentIndex > 0)
        {
            currentIndex = parentIndex;
            parentIndex = (parentIndex - 1) / BranchingFactor;

            if (pairs[parentIndex].Priority < pairs[currentIndex].Priority)
                Swap(currentIndex, parentIndex);
            else
                break;

            Print(currentIndex, parentIndex);
        }
    }

    public void BubbleUp()
        => BubbleUp(pairs.Length - 1);

    private void Swap(int left, int right)
    {
        swapSpace = pairs[left];
        pairs[left] = pairs[right];
        pairs[right] = swapSpace;
    }

    public void Print(int current = 0, int parent = 0)
    {
        Console.WriteLine($"parent:[{parent}]={pairs[parent].Priority}\tcurrent:[{current}]={pairs[current].Priority}\t[{string.Join(',', pairs.Select(x => x.Priority))}]");
    }
}