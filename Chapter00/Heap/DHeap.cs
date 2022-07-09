namespace Chapter00.Heap
{
    public class DHeap<T>
    {
        public const int None = -1;
        private readonly int branchingFactor;

        private PriorityNode[] nodes;

        private struct PriorityNode
        {
            internal PriorityNode(T element, int priority)
            {
                Priority = priority;
                Element = element;
            }

            internal readonly T Element;
            internal int Priority;

            public void Deconstruct(out T element, out int priority)
            {
                element = Element;
                priority = Priority;
            }
        }

        protected DHeap(IEnumerable<(T Element, int Priority)> array, int branchingFactor)
        {
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

                if (newPriority > oldPriority)
                {
                    Console.WriteLine("BubblingUp Priority");
                    BubbleUp(index);
                }
                else if (newPriority < oldPriority)
                {
                    Console.WriteLine("PushingDown Priority");
                    PushDown(index);
                }
            }
        }

        /// <summary>
        /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/150
        /// </summary>
        /// <param name="index">The index of the element to begin with.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
            int firstLeafIndex = (nodes.Length - 2) / (branchingFactor + 1);

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
