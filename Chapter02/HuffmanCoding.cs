using Chapter00.Heap;

namespace Chapter02
{
    public class HuffmanNode
    {
        public HuffmanNode(string value, int frequency)
        {
            Value = value;
            Frequency = frequency;
        }

        public HuffmanNode? Left;
        public HuffmanNode? Right;
        public string Value;
        public int Frequency;

        public bool Validate()
        {
            var leftSymbols = Left?.Value ?? "";
            var rightSymbols = Right?.Value ?? "";

            var leftFrequency = Left?.Frequency ?? 0;
            var rightFrequency = Right?.Frequency ?? 0;

            if (Value != leftSymbols + rightSymbols)
                return false;

            if (Frequency != leftFrequency + rightFrequency)
                return false;

            return true;
        }

        public override string ToString()
        {
            return $"Value:{Value} Freq:{Frequency}";
        }
    }

    /// <summary>
    /// Huffman's Algorithm for data compression.
    /// https://livebook.manning.com/book/algorithms-and-data-structures-in-action/chapter-2/341
    /// </summary>
    /// <remarks>2.8.3 Data compression: Huffman codes</remarks>
    public static class HuffmanCoding
    {
        /// <summary>
        /// Create a Huffman encoding for a text.
        /// </summary>
        /// <param name="text">The input string to be compressed.</param>
        /// <returns>
        /// A dictionary with an entry for each unique character in the text.
        /// Each entry is a string representation of the binary sequence that encodes the character.
        /// So, if ('a', '101') is in the output dictionary, to compress the original text one should
        /// replace all occurrences of 'a' in the text with the 3 bits 101 (using binary arithmetic).
        /// </returns>
        public static Dictionary<char, string> Huffman(string text)
        {
            var charFrequenciesMap = ComputeFrequencies(text);
            var priorityQueue = FrequencyToHeap(charFrequenciesMap);
            var tree = HeapToTree(priorityQueue);

            return CreateEncoding(tree, string.Empty, new Dictionary<char, string>());
        }

        /// <summary>
        /// Given a text (a string), creates a dictionary with chars/number of occurrences.
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>The counts of each character to encode.</returns>
        public static Dictionary<char, int> ComputeFrequencies(string text)
        {
            var frequencies = new Dictionary<char, int>();
            for (var i = 0; i < text.Length; i++)
            {
                if (!frequencies.ContainsKey(text[i]))
                {
                    frequencies.Add(text[i], 1);
                }
                else
                {
                    frequencies[text[i]] += 1;
                }
            }

            return frequencies;
        }

        /// <summary>
        /// Takes a frequency table and creates a heap whose elements are nodes of the Huffman tree,
        /// with one node per unique character in the FT; for each element the priority associated to it is
        /// the frequency of the corresponding character.
        /// </summary>
        /// <param name="charFrequenciesMap">
        /// The frequency table, with char/number of occurrences (or document frequency) pairs.
        /// branching_factor: The branching factor for the d-ary heap that will be created.
        /// </param>
        /// <returns>
        /// A d-ary heap containing one entry per unique character in the original text;
        /// Each entry is going to be an instance of `HuffmanNode`.
        /// </returns>
        public static DHeap<HuffmanNode> FrequencyToHeap(Dictionary<char, int> charFrequenciesMap)
        {
            var priorityQueue = new DHeap<HuffmanNode>();
            foreach (var (chars, frequency) in charFrequenciesMap)
            {
                priorityQueue.Insert(new HuffmanNode(chars.ToString(), -1 * frequency), -1 * frequency);
            }

            return priorityQueue;
        }

        /// <summary>
        /// Builds a Huffman encoding tree from a priority queue (containing entries for each of the unique characters in the text to be compressed.
        /// </summary>
        /// <param name="heap">The heap.</param>
        /// <returns>The root of the Huffman encoding tree.</returns>
        public static HuffmanNode HeapToTree(DHeap<HuffmanNode> heap)
        {
            while (heap.Size > 1)
            {
                var right = heap.Top();
                var left = heap.Top();

                var parent = new HuffmanNode(left.Element.Value + right.Element.Value, left.Priority + right.Priority)
                {
                    Left = left.Element,
                    Right = right.Element
                };

                heap.Insert(parent, parent.Frequency);
            }

            return heap.Top().Element;
        }

        /// <summary>
        /// The Huffman coding algorithm (building a table from the tree)
        /// </summary>
        private static Dictionary<char, string> CreateEncoding(HuffmanNode node, string sequence, Dictionary<char, string> charactersToSequenceMap)
        {
            if (node.Value.Length == 1)
            {
                charactersToSequenceMap[node.Value[0]] = sequence;
            }
            else
            {
                if (node.Left is not null)
                {
                    CreateEncoding(node.Left, sequence + "0", charactersToSequenceMap);
                }

                if (node.Right is not null)
                {
                    CreateEncoding(node.Right, sequence + "1", charactersToSequenceMap);
                }
            }

            return charactersToSequenceMap;
        }
    }
}
