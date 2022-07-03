// Resources: https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/datastructures/heap/dway_heap.py#L169
// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/184

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

Console.WriteLine("PushDown");
heap.Print();
heap.BubbleUp(tasks.Length - 1);
heap.Print();

// Figure 2.8
Console.WriteLine();
Console.WriteLine("PushDown");
tasks = new Pair[8]
{
    new Pair(9, "UI breaks on browser X"),
    new Pair(10, "Unencrypted password on DB"),
    new Pair(8, "Optional form field blocked"),
    new Pair(8, "CSS style causes misalignment"),
    new Pair(7, "Page loads take 2+ seconds"),
    new Pair(5, "CSS style causes 1px offset"),
    new Pair(3, "Refactor CSS using SASS"),
    new Pair(9, "Memory leak")
};

heap = new DHeap(tasks);
heap.Print();
heap.PushDown(0);
heap.Print();

public record Pair(int Priority, string Element);


public class DHeap
{
    public const int None = -1;
    private const int BranchingFactor = 3;

    private readonly Pair[] pairs;
    private readonly int firstLeafIndex;

    private Pair? swapSpace;

    public DHeap(params Pair[] pairs)
    {
        this.pairs = pairs;
        firstLeafIndex = (pairs.Length - 2) / (BranchingFactor + 1);
    }

    /// <summary>
    /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/150
    /// </summary>
    /// <param name="index">The index of the element to begin with.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void BubbleUp(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
        if (index > pairs.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

        static int getParentIndex(int x) => (x - 1) / BranchingFactor;
        int parentIndex;

        var current = pairs[index];
        while (index > 0)
        {
            parentIndex = getParentIndex(index);
            if (pairs[parentIndex].Priority < current.Priority)
            {
                pairs[index] = pairs[parentIndex];
                index = parentIndex;
                Print(parentIndex, index);
            }
            else
            {
                break;
            }
        }

        pairs[index] = current;
    }

    /// <summary>
    /// The PushDown method handles the symmetric case where we need to move an element down toward the 
    /// leaves of the heap, because it might be larger than (at least) one of its children. 
    /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/170
    /// </summary>
    /// <param name="index">The index of the element to begin with.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PushDown(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
        if (index > pairs.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

        var currentIndex = index;
        while (currentIndex < firstLeafIndex)
        {
            var childIndex = GetHighestPriorityChildIndex(currentIndex);
            if (childIndex == None)
            {
                Console.WriteLine("Get None during pushdown.");
                break;
            }

            if (pairs[childIndex].Priority > pairs[currentIndex].Priority)
            {
                Swap(currentIndex, childIndex);
                currentIndex = childIndex;
                Print(currentIndex, childIndex);
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Gets the index of the highest priority child of for the node at the given index.
    /// </summary>
    /// <param name="index">The index of the heap node for which to find the highest priority child.</param>
    /// <returns>Returns -1 if the current node has no chilren</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private int GetHighestPriorityChildIndex(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
        if (index > pairs.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

        var first = GetFirstChildIndex(index);
        var last = Math.Min(first + BranchingFactor, pairs.Length);

        if (first > pairs.Length)
            return None;

        var highest = int.MinValue;
        for (int i = first; i < last; i++)
        {
            if (pairs[i].Priority > highest)
            {
                highest = pairs[i].Priority;
                index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// Gets the index of the first child node of a heap node by it's index.
    /// </summary>
    /// <param name="index">the index of the node for which to find the first child.</param>
    /// <returns>The index of the left-most child index of the given index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private int GetFirstChildIndex(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
        if (index > pairs.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

        return index * BranchingFactor + 1;
    }

    private void Swap(int firstIndex, int secondIndex)
    {
        swapSpace = pairs[firstIndex];
        pairs[firstIndex] = pairs[secondIndex];
        pairs[secondIndex] = swapSpace;
    }

    public void Print(int current = 0, int parent = 0, Action<string>? write = default)
    {
        write ??= Console.WriteLine;
        write($"parent:[{parent}]={pairs[parent].Priority}\tcurrent:[{current}]={pairs[current].Priority}\t[{string.Join(',', pairs.Select(x => x.Priority))}]");
    }
}