using System.Xml.Linq;

namespace Chapter00.Heap
{
    /// <summary>
    /// https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/datastructures/heap/dway_heap.py#L51
    /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/374
    /// **Implementation of a d-ary heap.**
    /// The branching factor for the heap can be passed as an argument.
    /// It's 2 by default, which is also the minimum possible value.
    /// The branching factor is the maximum number of children that each internal node can have.
    /// For regular heaps, a node an have at most 2 children, so the branching factor is 2.
    /// The higher the branching factor, the shortest the height of the heap. However, when an element is
    /// pushed towards the leaves of the heap, at each step all children of current node must be examined,
    /// so a larger branching factor implies a higher number of nodes to be checked for each step of this
    /// operation.
    /// On the other hand, inserting elements only examines at most h element, where h is the height of the heap,
    /// so this operation is only made faster with larger branching factors.
    /// In general values between 3 and 5 are a good compromise and produce good performance."""
    /// </summary>
    /// <typeparam name="T">The type of heap element. These items get priority.</typeparam>
    public class DHeap<T>
    {
        public const int None = -1;
        private readonly int branchingFactor;

        private PriorityNode[] nodes;

        private struct PriorityNode
        {
            internal PriorityNode(T element, int priority)
            {
                Element = element;
                Priority = priority;
            }

            internal readonly T Element;
            internal int Priority;

            public void Deconstruct(out T element, out int priority)
            {
                element = Element;
                priority = Priority;
            }

            public override string ToString()
            {
                return $"Priority:{Priority} Value:{Element}";
            }
        }

        protected DHeap(IEnumerable<(T Element, int Priority)> array, int branchingFactor)
        {
            if (branchingFactor < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(branchingFactor), $"Branching factor ({branchingFactor}) must be greater than 1.");
            }


            this.branchingFactor = branchingFactor;

            nodes = array
                .Select(x => new PriorityNode(x.Element, x.Priority))
                .ToArray();

            if (nodes.Length > 0)
            {
                for (var index = (nodes.Length - 1) / branchingFactor; index >= 0; index--)
                {
                    PushDown(index);
                }
            }
        }

        public DHeap(int branchingFactor = 3, params (T Element, int Priority)[] array)
            : this(array.AsEnumerable(), branchingFactor)
        {
        }

        public DHeap(params (T Element, int Priority)[] array)
            : this(array.AsEnumerable(), 3)
        {
        }

        public int Size => nodes.Length;

        public DHeap<T> TopK(int k)
        {
            var heap = new DHeap<T>();
            foreach (var el in nodes)
            {
                if (heap.Size == k && heap.Peek().Priority < el.Priority)
                {
                    _ = heap.Top();
                }
                if (heap.Size < k)
                {
                    heap.Insert(el.Element, el.Priority);
                }
            }

            return heap;
        }

        public (T Element, int Priority) Peek()
        {
            if (nodes.Length == 0)
            {
                throw new InvalidOperationException($"{nameof(DHeap<T>)} is empty");
            }

            return (nodes[0].Element, nodes[0].Priority);
        }

        public (T Element, int Priority) Top()
        {
            PriorityNode temp;
            if (nodes.Length == 0)
            {
                throw new InvalidOperationException($"{nameof(DHeap<T>)} is empty");
            }

            temp = nodes[^1];
            Array.Resize(ref nodes, nodes.Length - 1);
            if (nodes.Length == 0)
            {
                return (temp.Element, temp.Priority);
            }
            else
            {
                var (Element, Priority) = nodes[0];
                nodes[0] = temp;
                PushDown();
                return (Element, Priority);
            }
        }

        public void Insert(T element, int priority)
        {
            Array.Resize(ref nodes, nodes.Length + 1);
            nodes[^1] = new PriorityNode(element, priority);
            BubbleUp(nodes.Length - 1);
        }

        public void Update(T oldValue, int newPriority)
        {
            ArgumentNullException.ThrowIfNull(oldValue as object);

            int index;
            for (index = 0; index < nodes.Length; index++)
            {
                if (nodes[index].Element.Equals(oldValue))
                {
                    break;
                }
            }

            Console.WriteLine($"Found item at index {index}");

            if (index >= 0)
            {
                var oldPriority = nodes[index].Priority;
                nodes[index] = nodes[index] with { Priority = newPriority };

                if (newPriority < oldPriority)
                {
                    Console.WriteLine("BubblingUp Priority");
                    BubbleUp(index);
                }
                else if (newPriority > oldPriority)
                {
                    Console.WriteLine("PushingDown Priority");
                    PushDown(index);
                }
            }
        }


        /// <summary>
        /// TODO: https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/datastructures/heap/dway_heap.py#L51
        /// Checks that the three invariants for heaps are abided by.
        /// 1.	Every node has at most `branchingFactor` children. (Guaranteed by construction)
        /// 2.	The heap tree is complete and left-adjusted.(Also guaranteed by construction)
        /// 3.	Every node holds the highest priority in the subtree rooted at that node.
        /// </summary>
        /// <returns>True if all the heap invariants are met, false otherwise.</returns>
        public bool Validate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bubbles up towards the root an element, to reinstate heap's invariants.
        /// If the parent P of an element has lower priority, then swaps current element and its parent,
        /// and then recursively check the position previously held by the P.
        /// </summary>
        /// <param name="index">The index of the element to bubble up.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <remarks>reference https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/150
        public void BubbleUp(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
            if (index > nodes.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

            int parentIndex;
            var current = nodes[index];
            while (index > 0)
            {
                parentIndex = (index - 1) / branchingFactor;
                if (nodes[parentIndex].Priority < current.Priority)
                {
                    nodes[index] = nodes[parentIndex];
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }

            nodes[index] = current;
        }

        /// <summary>
        /// The PushDown method handles the symmetric case where we need to move an element down toward the 
        /// leaves of the heap, because it might be larger than (at least) one of its children. 
        /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/170
        /// </summary>
        /// <param name="index">The index of the element to begin with.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void PushDown(int index = 0)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
            if (index > nodes.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

            var current = nodes[index];
            int firstLeafIndex = ((nodes.Length - 2) / branchingFactor) + 1;

            while (index < firstLeafIndex)
            {
                var childIndex = GetHighestPriorityChildIndex(index);
                if (childIndex == None)
                {
                    throw new Exception("ChildIndex is None");
                }

                if (nodes[childIndex].Priority > nodes[index].Priority)
                {
                    nodes[index] = nodes[childIndex];
                    index = childIndex;
                }
                else
                {
                    break;
                }
            }

            nodes[index] = current;
        }

        /// <summary>
        /// Gets the index of the highest priority child of for the node at the given index.
        /// </summary>
        /// <param name="index">The index of the heap node for which to find the highest priority child.</param>
        /// <returns>Returns -1 if the current node has no chilren</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <remarks>reference code https://github.com/mlarocca/AlgorithmsAndDataStructuresInAction/blob/7a5b7a7a2b84257c99c28f6b92e47141f844afc9/Python/mlarocca/datastructures/heap/dway_heap.py#L143</remarks>
        private int GetHighestPriorityChildIndex(int index)
        {
            var start = index;
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "cannot be less than 0");
            if (index > nodes.Length - 1) throw new ArgumentOutOfRangeException(nameof(index), "cannot be greater than array length - 1.");

            var firstChildIndex = index * branchingFactor + 1;
            var lastChildIndex = Math.Min(firstChildIndex + branchingFactor, nodes.Length);

            if (firstChildIndex > nodes.Length)
                return None;

            var highest = int.MinValue;
            for (int i = firstChildIndex; i < lastChildIndex; i++)
            {
                if (nodes[i].Priority > highest)
                {
                    highest = nodes[i].Priority;
                    index = i;
                }
            }

            Console.WriteLine($"Highest Priority Child of index:{start} is {index}");
            return index;
        }

        public void Print(int current = 0, int parent = 0, Action<string>? write = default)
        {
            write ??= Console.WriteLine;
            write($"parent:[{parent}]={nodes[parent].Priority}\tcurrent:[{current}]={nodes[current].Priority}\t[{string.Join(',', nodes.Select(x => x.Priority))}]");
        }
    }
}
